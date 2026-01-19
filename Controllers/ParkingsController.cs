using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Data;
using Garage_2._0.Models;

namespace Garage_2._0.Controllers
{
    public class ParkingsController : Controller
    {
        private readonly Garage_2_0Context _context;

        public ParkingsController(Garage_2_0Context context)
        {
            _context = context;
        }

        // GET: Parkings
        public async Task<IActionResult> Index()
        {
            var garage_2_0Context = _context.Parkings.Include(p => p.ParkingSpot).Include(p => p.Vehicle);
            return View(await garage_2_0Context.ToListAsync());
        }

        // GET: Parkings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings
                .Include(p => p.ParkingSpot)
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }

            return View(parking);
        }

        // GET: Parkings/Create
        public IActionResult Create()
        {
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id");
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber");
            return View();
        }

        // POST: Parkings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleId,ParkingSpotId,StartTime,EndTime")] Parking parking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parking.ParkingSpotId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber", parking.VehicleId);
            return View(parking);
        }

        // GET: Parkings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings.FindAsync(id);
            if (parking == null)
            {
                return NotFound();
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parking.ParkingSpotId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber", parking.VehicleId);
            return View(parking);
        }

        // POST: Parkings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleId,ParkingSpotId,StartTime,EndTime")] Parking parking)
        {
            if (id != parking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingExists(parking.Id))
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
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parking.ParkingSpotId);
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "RegistrationNumber", parking.VehicleId);
            return View(parking);
        }

        // GET: Parkings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parking = await _context.Parkings
                .Include(p => p.ParkingSpot)
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parking == null)
            {
                return NotFound();
            }

            return View(parking);
        }

        // POST: Parkings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parking = await _context.Parkings.FindAsync(id);
            if (parking != null)
            {
                _context.Parkings.Remove(parking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingExists(int id)
        {
            return _context.Parkings.Any(e => e.Id == id);
        }
    }
}
