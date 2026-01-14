using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Garage_2._0.Data;
using Microsoft.AspNetCore.Identity;
using Garage_2._0.Models;
namespace Garage_2._0
{
    // TEST - Git Demo
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Garage_2_0Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Garage_2_0Context") ?? throw new InvalidOperationException("Connection string 'Garage_2_0Context' not found.")));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Garage_2_0Context>();

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

            app.Run();
        }
    }
}
