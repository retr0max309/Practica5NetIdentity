using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetIdentity.Models;
using Claim = System.Security.Claims.Claim;

namespace NetIdentity.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            await context.Database.ExecuteSqlRawAsync(@"
IF COL_LENGTH('dbo.AspNetUsers','genero') IS NULL
    ALTER TABLE dbo.AspNetUsers ADD genero NVARCHAR(256) NULL;
IF COL_LENGTH('dbo.AspNetUsers','fecha_nacimiento') IS NULL
    ALTER TABLE dbo.AspNetUsers ADD fecha_nacimiento DATETIME2 NULL;
IF COL_LENGTH('dbo.AspNetUsers','nombre_completo') IS NULL
    ALTER TABLE dbo.AspNetUsers ADD nombre_completo NVARCHAR(256) NULL;
");

            string[] roleNames = { "Admin", "Usuario" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (await userManager.FindByEmailAsync("admin@test.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-30),
                    NombreCompleto = "Administrador Sistema",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    var fn = adminUser.FechaNacimiento?.ToString("yyyy-MM-dd") ?? "";
                    await userManager.AddClaimAsync(adminUser, new Claim("FechaNacimiento", fn));
                }
            }

            if (await userManager.FindByEmailAsync("menor@test.com") == null)
            {
                var userMenor = new ApplicationUser
                {
                    UserName = "menor@test.com",
                    Email = "menor@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-15),
                    NombreCompleto = "Juan Menor",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(userMenor, "Menor123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMenor, "Usuario");
                    var fn = userMenor.FechaNacimiento?.ToString("yyyy-MM-dd") ?? "";
                    await userManager.AddClaimAsync(userMenor, new Claim("FechaNacimiento", fn));
                }
            }

            if (await userManager.FindByEmailAsync("mayor@test.com") == null)
            {
                var userMayor = new ApplicationUser
                {
                    UserName = "mayor@test.com",
                    Email = "mayor@test.com",
                    FechaNacimiento = DateTime.Now.AddYears(-25),
                    NombreCompleto = "María Mayor",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(userMayor, "Mayor123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userMayor, "Usuario");
                    var fn = userMayor.FechaNacimiento?.ToString("yyyy-MM-dd") ?? "";
                    await userManager.AddClaimAsync(userMayor, new Claim("FechaNacimiento", fn));
                }
            }
        }
    }
}
