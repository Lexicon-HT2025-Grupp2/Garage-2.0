using Garage_2._0.Data;

using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Configuration;

namespace Garage_2._0.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly PricingService _pricingService;
        private readonly int _totalSpots;

        public ParkedVehiclesController(Garage_2_0Context context, PricingService pricing, IOptions<GarageSettings> settings)
        {
            _context = context;
            _pricingService = pricing;
            _totalSpots = settings.Value.TotalSpots;

        }

        static private List<SelectListItem> MakeEnumList<TEnum>() where TEnum : Enum
        {
            List<SelectListItem> vl = new();
            foreach (var v in Enum.GetValues(typeof(TEnum)))
            {
                SelectListItem sli = new();
                sli.Value = sli.Text = v.ToString();
                vl.Add(sli);
            }
            return new(vl);
        }



        // GET: ParkedVehicles
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalSpots = _totalSpots;
            ViewBag.FreeSpots = _totalSpots - await _context.ParkedVehicle.CountAsync();
            return View(await ParkedVehiclesQuery().ToListAsync());
        }

        // GET: ParkedVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/CheckIn
        public IActionResult CheckIn()
        {
            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            return View();
        }

        // POST: ParkedVehicles/CheckIn
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn([Bind("Id,RegistrationNumber,Type,Color,Brand,Model,NumberOfWheels,ArrivalTime,Note")] ParkedVehicle parkedVehicle)
        {
            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            if (!ModelState.IsValid)
                return View(parkedVehicle);

            var exists = await _context.ParkedVehicle
                .AnyAsync(v => v.RegistrationNumber == parkedVehicle.RegistrationNumber);

            if (exists)
            {
                TempData["ErrorMessage"] = "Failed to enter vehicle into garage.";
                ModelState.AddModelError(nameof(ParkedVehicle.RegistrationNumber),
                    "Car already exists in the garage.");

                return View(parkedVehicle);
            }

            var occupied = await _context.ParkedVehicle.CountAsync();

            if (occupied >= _totalSpots)
            {
                TempData["ErrorMessage"] = "Garaget är fullt. Inga lediga platser.";
                return RedirectToAction(nameof(Index));
            }


            parkedVehicle.SpotNumber = GetNextFreeSpot();
            _context.Add(parkedVehicle);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Vehicle entered garage successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: ParkedVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }
            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();
            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationNumber,Type,Color,Brand,Model,NumberOfWheels,ArrivalTime,Note")] ParkedVehicle parkedVehicle)
        {
            if (id != parkedVehicle.Id)
            {
                return NotFound();
            }

            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();

            var exists = await _context.ParkedVehicle
                .AnyAsync(v => v.RegistrationNumber == parkedVehicle.RegistrationNumber);

            if (exists)
            {
                TempData["ErrorMessage"] = "Failed to enter vehicle into garage.";
                ModelState.AddModelError(nameof(ParkedVehicle.RegistrationNumber),
                    "Vehicle already exists in the garage.");

                return View(parkedVehicle);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkedVehicle);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Parked vehicle was updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkedVehicleExists(parkedVehicle.Id))
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
            return View(parkedVehicle);
        }

        // GET: ParkedVehicles/CheckOut/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkedVehicle = await _context.ParkedVehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkedVehicle == null)
            {
                return NotFound();
            }

            return View(parkedVehicle);
        }

        // POST: ParkedVehicles/CheckOut/5
        // Removes a vehicle from the database and shows a receipt
        [HttpPost, ActionName("CheckOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutConfirmed(int id)
        {
            // Find the vehicle by id (int id)
            var parkedVehicle = await _context.ParkedVehicle.FindAsync(id);
            if (parkedVehicle != null)
            {
                // Remove vehicle from database
                _context.ParkedVehicle.Remove(parkedVehicle);
                await _context.SaveChangesAsync();

                // Calculate departure time
                var departureTime = DateTime.Now;

                // Use PricingService to calculate price
                var price = _pricingService.CalculatePrice(parkedVehicle.ArrivalTime, departureTime);

                // Build receipt view model
                var receipt = new ReceiptViewModel
                {
                    RegistrationNumber = parkedVehicle.RegistrationNumber,
                    ArrivalTime = parkedVehicle.ArrivalTime,
                    DepartureTime = departureTime,
                    TotalTime = departureTime - parkedVehicle.ArrivalTime,
                    Price = price
                };

                TempData["SuccessMessage"] = "Vehicle left the garage successfully.";
                // Show Receipt view instead of redirecting
                return View("Receipt", receipt);
            }

            // If vehicle not found, go back to list
            return RedirectToAction(nameof(Index));

        }

        private bool ParkedVehicleExists(int id)
        {
            return _context.ParkedVehicle.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }
            var results = await ParkedVehiclesQuery()
                .Where(pv => pv.RegistrationNumber.Contains(searchTerm))
                .ToListAsync();
            return View("Index", results);
        }

        private IQueryable<ParkedVehicleViewModel> ParkedVehiclesQuery()
        {
            return _context.ParkedVehicle.Select(pv => new ParkedVehicleViewModel
            {
                Id = pv.Id,
                RegistrationNumber = pv.RegistrationNumber,
                Type = pv.Type,
                ArrivalTime = pv.ArrivalTime,
                SpotNumber = pv.SpotNumber
            });
        }

        private int GetNextFreeSpot()
        {
            var usedSpots = _context.ParkedVehicle
                .Select(v => v.SpotNumber)
                .ToList();

            int spot = 1;
            while (usedSpots.Contains(spot))
                spot++;

            return spot;
        }

        public async Task<IActionResult> Statistics()
        {
            var vehicles = await _context.ParkedVehicle.ToListAsync();
            var pricing = new PricingService();

            double totalRevenue = 0;
            double totalHours = 0;

            var revenueByType = new Dictionary<VehicleType, double>();

            foreach (var v in vehicles)
            {
                var now = DateTime.Now;

                // Duration
                var duration = (now - v.ArrivalTime).TotalHours;
                totalHours += duration;

                // Revenue
                var revenue = pricing.CalculatePrice(v.ArrivalTime, now);
                totalRevenue += revenue;

                // Revenue by type
                if (!revenueByType.ContainsKey(v.Type))
                    revenueByType[v.Type] = 0;

                revenueByType[v.Type] += revenue;
            }

            var model = new GarageStatisticsViewModel
            {
                TotalVehicles = vehicles.Count,
                TotalWheels = vehicles.Sum(v => v.NumberOfWheels),
                VehiclesByType = vehicles
                    .GroupBy(v => v.Type)
                    .ToDictionary(g => g.Key, g => g.Count()),
                VehiclesByColor = vehicles
                    .GroupBy(v => v.Color)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TotalRevenue = Math.Round(totalRevenue, 2),
                AverageParkingDurationHours = vehicles.Count > 0
                    ? Math.Round(totalHours / vehicles.Count, 2)
                    : 0,
                RevenueByType = revenueByType
                    .ToDictionary(k => k.Key, v => Math.Round(v.Value, 2))
            };

            return View(model);
        }


    }
}
