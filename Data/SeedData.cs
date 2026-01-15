using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;

namespace Garage_2._0.Data;

public static class SeedData
{
    private static readonly string[] roles = [ "Admin", "Member" ];
    private static readonly ApplicationUser adminUser = new ApplicationUser
    {
        UserName = "admin@email.com",
        Email = "admin@email.com",
        EmailConfirmed = true,
        FirstName = "Admin",
        LastName = "Adminsson",
        Personnummer = "123456-7890",
    };
    private static readonly ApplicationUser memberUser = new ApplicationUser
    {
        UserName = "member@email.com",
        Email = "member@email.com",
        EmailConfirmed = true,
        FirstName = "Member",
        LastName = "Membersson",
        Personnummer = "098765-4321",
    };
    private static readonly string password = "Passw0rd!";

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded) {
                throw new Exception("Failed to seed roles");
            }
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await userManager.FindByEmailAsync(adminUser.Email!) == null)
        {
            var result = await userManager.CreateAsync(adminUser, password);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to seed user: {adminUser}");
            }

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        if (await userManager.FindByEmailAsync(memberUser.Email!) == null)
        {
            var result = await userManager.CreateAsync(memberUser, password);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to seed user: {memberUser}");
            }

            await userManager.AddToRoleAsync(memberUser, "Member");
        }
    }
}