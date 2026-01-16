using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage_2._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VehiclesController(Garage_2_0Context context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Vehicles
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var vehicles = _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .Where(v => v.OwnerId == userId);

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("RegistrationNumber,VehicleTypeId,Color,Brand,Model,NumberOfWheels,Note")] Vehicle vehicle)
        {
            var userId = _userManager.GetUserId(User);
            vehicle.OwnerId = userId;

            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            if (!ModelState.IsValid)
                return View(vehicle);

            var exists = await _context.Vehicles
                .AnyAsync(v => v.RegistrationNumber == vehicle.RegistrationNumber);

            if (exists)
            {
                TempData["ErrorMessage"] = "Failed to register vehicle.";
                ModelState.AddModelError(nameof(Vehicle.RegistrationNumber),
                    "Car has already been registered.");

                return View(vehicle);
            }

            _context.Add(vehicle);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Vehicle registered successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Vehicles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,VehicleTypeId,Color,Brand,Model,NumberOfWheels,Note")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            vehicle.OwnerId = userId;

            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", vehicle.VehicleTypeId);
            ViewBag.Colors = MakeEnumList<ConsoleColor>();

            var exists = await _context.Vehicles
                .AnyAsync(v => v.RegistrationNumber == vehicle.RegistrationNumber && v.Id != vehicle.Id);

            if (exists)
            {
                TempData["ErrorMessage"] = "Failed to register vehicle.";
                ModelState.AddModelError(nameof(Vehicle.RegistrationNumber),
                    "Registration number already exists.");

                return View(vehicle);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vehicle updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Owner)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }

        private static List<SelectListItem> MakeEnumList<TEnum>() where TEnum : Enum
        {
            List<SelectListItem> vl = [];

            foreach (var v in Enum.GetValues(typeof(TEnum)))
            {
                SelectListItem sli = new();
                sli.Value = sli.Text = v.ToString();
                vl.Add(sli);
            }

            return [.. vl];
        }
    }
}
