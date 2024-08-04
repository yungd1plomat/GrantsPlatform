using GrantsPlatform.Abstractions;
using GrantsPlatform.Data;
using GrantsPlatform.Helpers;
using GrantsPlatform.Helpers.Converters;
using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;
using GrantsPlatform.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;
using System;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace GrantsPlatform
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Читаем конфиг
            var connectionString = builder.Configuration["POSTGRES_CONNECTION"] ?? throw new InvalidOperationException("Connection string not found.");
            var issuer = builder.Configuration["ISSUER"] ?? throw new InvalidOperationException("Issuer not found.");
            var secretKey = builder.Configuration["SECRET_KEY"] ?? throw new InvalidOperationException("Secret key not found.");
            var audience = builder.Configuration["AUDIENCE"] ?? throw new InvalidOperationException("Audience not found.");
            var expireDays = builder.Configuration.GetValue<int>("EXPIRE_DAYS");

            var jwtOptions = new JwtOptions(secretKey, issuer, audience, expireDays);
            builder.Services.AddSingleton(options => jwtOptions);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGrantRepository, GrantRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = jwtOptions.SymmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                };
            });

            // Чтобы маппить IEnumerable<int> в jsonb используем EnableDynamicJson
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(new NpgsqlDataSourceBuilder(connectionString)
                                    .EnableDynamicJson()
                                    .Build()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 2;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            
            builder.Services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new FilterMappingDtoConverter());
                options.JsonSerializerOptions.Converters.Add(new FilterConverter());
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });


            var app = builder.Build();

            // Автоматически добавляет тестовые данные из файла testData.json
            // Раскомментировать, если понадобится
            /*
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var options = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new FilterConverter(),
                        new FilterMappingDtoConverter()
                    },
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                var rawData = File.ReadAllText("testData.json");
                var grantsResponse = JsonSerializer.Deserialize<GrantsResponse>(rawData, options);
                var grants = grantsResponse.Grants.Select(gr => gr.FromDto());
                if (!db.Grants.Any())
                {
                    await db.AddRangeAsync(grants);
                }
                if (!db.FilterMappings.Any())
                {
                    var filtersMapping = grantsResponse.FiltersMapping.FromDto();
                    await db.FilterMappings.AddRangeAsync(filtersMapping);
                    if (!db.Filters.Any())
                    {
                        var filters = grantsResponse.FiltersMapping.FromDto()
                            .Select(x => x.Filter);
                        await db.Filters.AddRangeAsync(filters);
                    }
                }
                await db.SaveChangesAsync();
            }*/

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Кидаем ErrorResponse при ошибке авторизации
            app.UseMiddleware<CustomUnauthorizedMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToController("Index", "Home");

            app.Run();
        }
    }
}
