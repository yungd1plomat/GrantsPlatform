using GrantsPlatform.Data.Configurations;
using GrantsPlatform.Helpers.Converters;
using GrantsPlatform.Models;
using GrantsPlatform.Models.Viewmodels.Grant;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System.Text.Json;

namespace GrantsPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Grant> Grants { get; set; }

        public DbSet<FilterMapping> FilterMappings { get; set; }

        public DbSet<Filter> Filters { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GrantConfiguration());
            modelBuilder.ApplyConfiguration(new FilterMappingConfiguration());
            modelBuilder.ApplyConfiguration(new FilterConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
