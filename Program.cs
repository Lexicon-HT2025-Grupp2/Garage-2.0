//using Garage_2._0.Data;
//using Garage_2._0.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Garage_2._0
//{
//    // TEST - Git Demo
//    public class Program
//    {
//        public static async Task Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);
//            builder.Services.AddDbContext<Garage_2_0Context>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("Garage_2_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_2_0Context' not found.")));

//            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Garage_2_0Context>();

//            builder.Services
//                .AddIdentity<ApplicationUser, IdentityRole>(options =>
//                {
//                    options.SignIn.RequireConfirmedAccount = false;
//                })
//                .AddEntityFrameworkStores<Garage_2_0Context>()
//                .AddDefaultTokenProviders();

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            builder.Services.AddRazorPages();
//            //Register application services
//            builder.Services.AddScoped<Services.PricingService>();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }
//            else
//            {
//                await SeedData.SeedAsync(app.Services);
//            }

//            //using(var scope = app.Services.CreateScope())
//            //{
//            //    var db = scope.ServiceProvider.GetRequiredService<Garage_2_0Context>();
//            //    db.Database.Migrate();
//            //}

//            app.UseHttpsRedirection();
//            app.UseRouting();

//            app.UseAuthentication();
//            app.UseAuthorization();
//            app.MapRazorPages();

//            app.MapStaticAssets();
//            app.MapControllerRoute(
//                name: "default",
//                pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}")
//                .WithStaticAssets();

//            app.Run();
//        }
//    }
//}
using Garage_2._0.Data;
using Garage_2._0.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ----------------------------
            // Database
            // ----------------------------
            builder.Services.AddDbContext<Garage_2_0Context>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Garage_2_0Context")
                    ?? throw new InvalidOperationException("Connection string 'Garage_2_0Context' not found.")
                ));

            // ----------------------------
            // Identity (Users + Roles)
            // ----------------------------
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    // Optional: basic password policy (adjust as you like)
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<Garage_2_0Context>()
                .AddDefaultTokenProviders();

            // ----------------------------
            // MVC + Razor Pages (Identity UI)
            // ----------------------------
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // ----------------------------
            // App Services
            // ----------------------------
            builder.Services.AddScoped<Services.PricingService>();

            var app = builder.Build();

            // ----------------------------
            // Seed (Roles, Admin, Demo data)
            // Run this in ALL environments if you want the DB always ready.
            // If you want it only in dev, wrap with: if (app.Environment.IsDevelopment()) { ... }
            // ----------------------------
            await SeedData.SeedAsync(app.Services);

            // ----------------------------
            // HTTP pipeline
            // ----------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // ----------------------------
            // Endpoints
            // ----------------------------
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}

