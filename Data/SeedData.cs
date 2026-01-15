using Microsoft.AspNetCore.Identity;

namespace Garage_2._0.Data;

public static class SeedData
{
    private static readonly string[] roles = [ "Admin", "Member" ];

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
    }
}