using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly PricingService _pricing;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(
            Garage_2_0Context context,
            PricingService pricing,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _pricing = pricing;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search)
        {
            // Load all users, optionally filtered by search text (performed in the database)
            var query = _context.Users.AsQueryable();



            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                // Search by first name, last name or personnummer
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(s) ||
                    u.LastName.ToLower().Contains(s) ||
                    u.Personnummer.ToLower().Contains(s));
            }


            // Execute the filtered user query
            var filteredUsers = await query.ToListAsync();
            var users = filteredUsers;
           

            // Build the overview model for each member
            var model = users.Select(u =>
            {
                // Load all vehicles owned by this member
                var vehicles = _context.Vehicles
                    .Where(v => v.OwnerId == u.Id)
                    .ToList();

                // Calculate total parking cost for all vehicles
                var totalCost = vehicles.Sum(v =>
                    _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));

                var roles = _userManager.GetRolesAsync(u).Result;
                var role = roles.FirstOrDefault() ?? "No role";

                return new UserOverviewVM
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Personnummer = u.Personnummer,
                    VehicleCount = vehicles.Count,
                    TotalCost = totalCost,
                    Role = role
                };
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> UserDetails(string id, string returnUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            var vehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.OwnerId == id)
            .ToListAsync();


            var totalCost = vehicles.Sum(v =>
                _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "No role";
            var model = new UserDetailsVM
            {
                User = user,
                Vehicles = vehicles,
                TotalCost = totalCost,
                Role = role
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
    }
}