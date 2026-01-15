using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly PricingService _pricing;

        public MembersController(Garage_2_0Context context, PricingService pricing)
        {
            _context = context;
            _pricing = pricing;
        }

        public async Task<IActionResult> Index(string search)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Personnummer.Contains(search));
            }

            var users = await usersQuery.ToListAsync();

            var model = users.Select(u =>
            {
                var vehicles = _context.ParkedVehicle
                    .Where(v => v.OwnerId == u.Id)
                    .ToList();

                var totalCost = vehicles.Sum(v => _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));

                return new MemberOverviewVM
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Personnummer = u.Personnummer,
                    VehicleCount = vehicles.Count,
                    TotalCost = totalCost
                };
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (member == null) return NotFound();

            var vehicles = await _context.ParkedVehicle
                .Where(v => v.OwnerId == id)
                .ToListAsync();

            var totalCost = vehicles.Sum(v => _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));

            var model = new MemberDetailsVM
            {
                Member = member,
                Vehicles = vehicles,
                TotalCost = totalCost
            };

            return View(model);
        }
    }
}