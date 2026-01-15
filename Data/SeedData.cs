using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data;

public static class SeedData
{
    private static readonly string[] roles = ["Admin", "Member"];
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
        var db = serviceProvider.GetRequiredService<Garage_2_0Context>();

        await EnsureVehicleTypesAsync(db);
        await db.SaveChangesAsync();
        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
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
        if (!await db.Vehicles.AnyAsync())
        {
            var carType = await db.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == "Car");

            db.Vehicles.Add(new Vehicle
            {
                RegistrationNumber = "ABC123",
                VehicleTypeId = carType!.Id,
                Color = "Red",
                Brand = "Volvo",
                Model = "XC40",
                NumberOfWheels = 4,
                ArrivalTime = DateTime.Now.AddHours(-2),
                Note = "Seeded vehicle",
                ParkingSpots = "1",
                OwnerId = memberUser.Id
            });

            await db.SaveChangesAsync();
        }

    }

    private static async Task EnsureVehicleTypesAsync(Garage_2_0Context db)
    {
        // Requires db.VehicleTypes DbSet
        if (!await db.Set<VehicleType>().AnyAsync())
        {
            db.Set<VehicleType>().AddRange(
                new VehicleType { Name = "Car" },
                new VehicleType { Name = "Motorcycle" },
                new VehicleType { Name = "Bus" },
                new VehicleType { Name = "Truck" },
                new VehicleType { Name = "Boat" },
                new VehicleType { Name = "Airplane" }
            );
        }
    }
}