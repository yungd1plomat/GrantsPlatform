using GrantsPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.Json;

#nullable disable

namespace GrantsPlatform.Data.Migrations
{
    public partial class SeedDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "correct_password")
            };

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: ["Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount", "PasswordHash"],
                values: ["1", defaultUser.UserName, defaultUser.NormalizedUserName, defaultUser.Email, defaultUser.NormalizedEmail, defaultUser.EmailConfirmed, defaultUser.PhoneNumberConfirmed, defaultUser.TwoFactorEnabled, defaultUser.LockoutEnabled, defaultUser.AccessFailedCount, defaultUser.PasswordHash]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1"
            );
        }
    }
}
