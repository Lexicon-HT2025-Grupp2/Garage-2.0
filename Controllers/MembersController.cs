using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
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
            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Personnummer.Contains(search));
            }

            var model = await users
                .Select(u => new MemberOverviewVM
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Personnummer = u.Personnummer,
                    //VehicleCount = _context.ParkedVehicle.Count(v => v.OwnerId == u.Id),
                    //TotalCost = _context.ParkedVehicle
                    //    .Where(v => v.OwnerId == u.Id)
                    //    .Sum(v => _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now))
                    VehicleCount = 0,
                    TotalCost = 0

                })
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (member == null) return NotFound();

            //var vehicles = await _context.ParkedVehicle
            //    .Where(v => v.OwnerId == id)
            //    .ToListAsync();
            //var totalCost = vehicles.Sum(v => _pricing.CalculatePrice(v.ArrivalTime, DateTime.Now));
            var vehicles = new List<ParkedVehicle>();
            var totalCost = 0;

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