namespace Garage_2._0.Data;

using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var db = scope.ServiceProvider.GetRequiredService<Garage_2_0Context>();

        // If you use migrations, this ensures DB is ready
        await db.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 1) Roles (required)
        await EnsureRolesAsync(roleManager);

        // 2) Admin user (required)
        var adminEmail = config["Seed:Admin:Email"] ?? "admin@garage.local";
        var adminPassword = config["Seed:Admin:Password"] ?? "Admin123!";
        await EnsureAdminUserAsync(userManager, adminEmail, adminPassword);

        // 3) Optional demo members (nice for testing)
        await EnsureDemoMembersAsync(userManager);

        // 4) Required domain seeds for Garage 3.0:
        // VehicleType table + ParkingSpot table
        //
        // IMPORTANT:
        // These DbSet names assume you added these entities in Garage 3.0:
        // - VehicleTypeEntity  -> db.VehicleTypes
        // - ParkingSpot        -> db.ParkingSpots
        //
        // If you haven't added them yet, add them first (Garage 3.0 requirement). :contentReference[oaicite:4]{index=4}
        await EnsureVehicleTypesAsync(db);
        await EnsureParkingSpotsAsync(db, config);

        await db.SaveChangesAsync();
    }

    private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Admin", "Member"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private static async Task EnsureAdminUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password)
    {
        var admin = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,

                // If you extend ApplicationUser in Garage 3.0 (recommended),
                // set these fields too:
                // FirstName = "Admin",
                // LastName = "User",
                // PersonalNumber = "19900101-1234",
                // DateOfBirth = new DateTime(1990, 1, 1),
                // MembershipType = "Pro",
                // MembershipValidUntil = DateTime.UtcNow.AddYears(10),
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin user: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
            await userManager.AddToRoleAsync(admin, "Admin");
    }

    private static async Task EnsureDemoMembersAsync(UserManager<ApplicationUser> userManager)
    {
        var demoUsers = new[]
        {
        new { Email = "member1@garage.local", Password = "Member123!", Pnr="20000101-1111", Dob=new DateTime(2000,1,1), First="Member", Last="One" },
        new { Email = "member2@garage.local", Password = "Member123!", Pnr="19980101-2222", Dob=new DateTime(1998,1,1), First="Member", Last="Two" }
    };

        foreach (var u in demoUsers)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == u.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = u.Email,
                    Email = u.Email,
                    EmailConfirmed = true,

                    FirstName = u.First,
                    LastName = u.Last,
                    PersonalNumber = u.Pnr,
                    DateOfBirth = u.Dob,

                    MembershipType = "Pro",
                    MembershipValidUntil = DateTime.UtcNow.AddDays(30)
                };

                var result = await userManager.CreateAsync(user, u.Password);
                if (!result.Succeeded)
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await userManager.IsInRoleAsync(user, "Member"))
                await userManager.AddToRoleAsync(user, "Member");
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

    private static async Task EnsureParkingSpotsAsync(Garage_2_0Context db, IConfiguration config)
    {
        // Requires db.ParkingSpots DbSet
        if (await db.Set<ParkingSpot>().AnyAsync())
            return;

        // Optional extra from assignment: read number of spots from config :contentReference[oaicite:5]{index=5}
        var totalSpots = config.GetValue("Garage:TotalSpots", 20);

        for (int i = 1; i <= totalSpots; i++)
        {
            db.Set<ParkingSpot>().Add(new ParkingSpot
            {
                SpotNumber = i,
                //Size = i <= totalSpots * 0.4 ? "Small"
                //     : i <= totalSpots * 0.8 ? "Medium"
                //     : "Large",
                //IsOccupied = false
            });
        }
    }
}
