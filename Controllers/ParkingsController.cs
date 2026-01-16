using Garage_2._0.Data;
using Garage_2._0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParkingsController : Controller
    {
        private readonly Garage_2_0Context _context;

        public ParkingsController(Garage_2_0Context context)
        {
            _context = context;
        }

        // GET: Parkings/Overview
        public async Task<IActionResult> Overview(string? vehicleType, string? reg)
        {
            var query = _context.Parkings
                .Include(p => p.Vehicle)!.ThenInclude(v => v.VehicleType)
                .Include(p => p.Vehicle)!.ThenInclude(v => v.Owner)
                .Include(p => p.ParkingSpot)
                .Where(p => p.EndTime == null);

            if (!string.IsNullOrWhiteSpace(vehicleType))
                query = query.Where(p => p.Vehicle!.VehicleType!.Name == vehicleType);

            if (!string.IsNullOrWhiteSpace(reg))
                query = query.Where(p => p.Vehicle!.RegistrationNumber.Contains(reg));

            var model = await query
                .Select(p => new ParkedVehicleOverviewVM
                {
                    ParkingId = p.Id,
                    OwnerName = p.Vehicle!.Owner!.FirstName + " " + p.Vehicle.Owner.LastName,
                    VehicleType = p.Vehicle.VehicleType!.Name,
                    RegistrationNumber = p.Vehicle.RegistrationNumber,
                    SpotNumber = p.ParkingSpot!.SpotNumber,
                    StartTime = p.StartTime
                })
                .ToListAsync();

            ViewBag.VehicleTypes = await _context.VehicleTypes
                .Select(vt => vt.Name)
                .ToListAsync();

            ViewBag.SelectedVehicleType = vehicleType;
            ViewBag.SearchReg = reg;

            return View(model);
        }
    }
}