using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Data;

public static class SeedData
{
    private static readonly string[] roles = ["Admin", "Member"];

    // Predefined admin user
    private static readonly ApplicationUser adminUser = new ApplicationUser
    {
        UserName = "admin@email.com",
        Email = "admin@email.com",
        EmailConfirmed = true,
        FirstName = "Admin",
        LastName = "Adminsson",
        Personnummer = "891212-7890",
    };

    // Predefined member user
    private static readonly ApplicationUser memberUser = new ApplicationUser
    {
        UserName = "member@email.com",
        Email = "member@email.com",
        EmailConfirmed = true,
        FirstName = "Member",
        LastName = "Membersson",
        Personnummer = "090316-4321",
    };

    private static readonly string password = "Passw0rd!";

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var db = serviceProvider.GetRequiredService<Garage_2_0Context>();

        // Seed vehicle types first
        await EnsureVehicleTypesAsync(db);
        await db.SaveChangesAsync();

        // Seed roles
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                    throw new Exception("Failed to seed roles");
            }
        }

        // Seed admin user
        if (await userManager.FindByEmailAsync(adminUser.Email!) == null)
        {
            var result = await userManager.CreateAsync(adminUser, password);

            if (!result.Succeeded)
                throw new Exception($"Failed to seed user: {adminUser}");

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Seed member user
        if (await userManager.FindByEmailAsync(memberUser.Email!) == null)
        {
            var result = await userManager.CreateAsync(memberUser, password);

            if (!result.Succeeded)
                throw new Exception($"Failed to seed user: {memberUser}");

            await userManager.AddToRoleAsync(memberUser, "Member");
        }

        // IMPORTANT:
        // Fetch the real user from the database to ensure the Id is populated.
        var realMemberUser = await userManager.FindByEmailAsync(memberUser.Email!);

        // Seed a vehicle for the member
        if (!await db.Vehicles.AnyAsync())
        {
            var carType = await db.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == "Car");

            // Add a vehicle linked to the member user
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

                // Correct way: use the real user from DB
                OwnerId = realMemberUser!.Id,
                Owner = realMemberUser
            });

            await db.SaveChangesAsync();
        }
    }

    private static async Task EnsureVehicleTypesAsync(Garage_2_0Context db)
    {
        // Seed vehicle types only if empty
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