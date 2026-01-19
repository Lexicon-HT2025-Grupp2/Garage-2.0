using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
namespace Garage_2._0
{
    // TEST - Git Demo
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Garage_2_0Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Garage_2_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_2_0Context' not found.")));

            //builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<Garage_2_0Context>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<Garage_2_0Context>()
                .AddDefaultTokenProviders();

            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IEmailSender, NoEmailSender>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Register application services
            builder.Services.AddScoped<Services.PricingService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                await SeedData.InitializeAsync(scope.ServiceProvider);
            }

            //using(var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<Garage_2_0Context>();
            //    db.Database.Migrate();
            //}

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=ParkedVehicles}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "parkingspots",
                pattern: "{controller=ParkingSpots}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "parkingspottypes",
                pattern: "{controller=ParkingSpotTypes}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "parkings",
                pattern: "{controller=Parkings}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}