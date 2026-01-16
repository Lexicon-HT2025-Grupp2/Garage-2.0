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
    public class MembersController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly PricingService _pricing;
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersController(
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

            // Keep only users who have the "Member" role
            var memberUsers = new List<ApplicationUser>();
            foreach (var user in filteredUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Member"))
                {
                    memberUsers.Add(user);
                }
            }

            // Build the overview model for each member
            var model = memberUsers.Select(u =>
            {
                // Load all vehicles owned by this member
                var vehicles = _context.Vehicles
                    .Where(v => v.OwnerId == u.Id)
                    .ToList();

                // Calculate total parking cost for all vehicles
                var totalCost = vehicles.Sum(v =>
                    _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));

                return new MemberOverviewVM
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Personnummer = u.Personnummer,
                    VehicleCount = vehicles.Count,
                    TotalCost = totalCost
                };
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> MemberDetails(string id, string returnUrl)
        {
            var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (member == null) return NotFound();

            var vehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.OwnerId == id)
            .ToListAsync();


            var totalCost = vehicles.Sum(v =>
                _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));

            var model = new MemberDetailsVM
            {
                Member = member,
                Vehicles = vehicles,
                TotalCost = totalCost
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
    }
}