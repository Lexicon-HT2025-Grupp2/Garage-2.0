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
    public class ParkingSpotTypesController : Controller
    {
        private readonly Garage_2_0Context _context;

        public ParkingSpotTypesController(Garage_2_0Context context)
        {
            _context = context;
        }

        // GET: ParkingSpotTypes
        public async Task<IActionResult> Index()
        {
            var garage_2_0Context = _context.ParkingSpotType.Include(p => p.PSpot).Include(p => p.VehicleType);
            return View(await garage_2_0Context.ToListAsync());
        }

        // GET: ParkingSpotTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotType = await _context.ParkingSpotType
                .Include(p => p.PSpot)
                .Include(p => p.VehicleType)
                .FirstOrDefaultAsync(m => m.ParkingSpotId == id);
            if (parkingSpotType == null)
            {
                return NotFound();
            }

            return View(parkingSpotType);
        }

        // GET: ParkingSpotTypes/Create
        public IActionResult Create()
        {
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
            return View();
        }

        // POST: ParkingSpotTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParkingSpotId,VehicleTypeId")] ParkingSpotType parkingSpotType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingSpotType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parkingSpotType.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", parkingSpotType.VehicleTypeId);
            return View(parkingSpotType);
        }

        // GET: ParkingSpotTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotType = await _context.ParkingSpotType.FindAsync(id);
            if (parkingSpotType == null)
            {
                return NotFound();
            }
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parkingSpotType.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", parkingSpotType.VehicleTypeId);
            return View(parkingSpotType);
        }

        // POST: ParkingSpotTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParkingSpotId,VehicleTypeId")] ParkingSpotType parkingSpotType)
        {
            if (id != parkingSpotType.ParkingSpotId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingSpotType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingSpotTypeExists(parkingSpotType.ParkingSpotId))
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
            ViewData["ParkingSpotId"] = new SelectList(_context.ParkingSpots, "Id", "Id", parkingSpotType.ParkingSpotId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", parkingSpotType.VehicleTypeId);
            return View(parkingSpotType);
        }

        // GET: ParkingSpotTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingSpotType = await _context.ParkingSpotType
                .Include(p => p.PSpot)
                .Include(p => p.VehicleType)
                .FirstOrDefaultAsync(m => m.ParkingSpotId == id);
            if (parkingSpotType == null)
            {
                return NotFound();
            }

            return View(parkingSpotType);
        }

        // POST: ParkingSpotTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingSpotType = await _context.ParkingSpotType.FindAsync(id);
            if (parkingSpotType != null)
            {
                _context.ParkingSpotType.Remove(parkingSpotType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingSpotTypeExists(int id)
        {
            return _context.ParkingSpotType.Any(e => e.ParkingSpotId == id);
        }
    }
}
