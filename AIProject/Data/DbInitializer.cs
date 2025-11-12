using System;
using System.Linq;
using AIProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var scopedProvider = scope.ServiceProvider;
            var context = scopedProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            const string adminRole = "Administrator";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            const string customerRole = "Customer";
            if (!await roleManager.RoleExistsAsync(customerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(customerRole));
            }

            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin#12345";

            var adminUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Administrator",
                    BirthDate = new DateTime(2000, 1, 1),
                    Department = "Administration"
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create seed administrator: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
            }

            if (string.IsNullOrWhiteSpace(adminUser.FirstName)
                || string.IsNullOrWhiteSpace(adminUser.LastName)
                || string.IsNullOrWhiteSpace(adminUser.Department)
                || adminUser.BirthDate == default)
            {
                adminUser.FirstName = string.IsNullOrWhiteSpace(adminUser.FirstName) ? "System" : adminUser.FirstName;
                adminUser.LastName = string.IsNullOrWhiteSpace(adminUser.LastName) ? "Administrator" : adminUser.LastName;
                adminUser.Department = string.IsNullOrWhiteSpace(adminUser.Department) ? "Administration" : adminUser.Department;
                adminUser.BirthDate = adminUser.BirthDate == default ? new DateTime(2000, 1, 1) : adminUser.BirthDate;
                await userManager.UpdateAsync(adminUser);
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminRole))
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}
