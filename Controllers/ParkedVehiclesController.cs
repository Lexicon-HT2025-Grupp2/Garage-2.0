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

        // Finds a suitable parking spot depending on the vehicle type
        private int FindAvailableSpot(ParkedVehicle vehicle)
        {
            return vehicle.Type switch
            {
                VehicleType.Motorcycle => FindSpotForMotorcycle(),
                VehicleType.Car => FindSpotForCar(),
                VehicleType.Truck => FindSpotForTruck(),
                VehicleType.Boat => FindSpotForLargeVehicle(),
                VehicleType.Airplane => FindSpotForLargeVehicle(),
                _ => -1
            };
        }

        // ---------------- MOTORCYCLE LOGIC ----------------
        // Motorcycles take 1/3 of a spot.
        // Up to 3 motorcycles can share the same spot.
        private int FindSpotForMotorcycle()
        {
            // 1. Try to find a partially occupied motorcycle spot (1 or 2 MC already)
            var sharedSpot = _context.ParkingSpot
                .Where(s => s.VehicleId == null && s.MotorcycleCount > 0 && s.MotorcycleCount < 3)
                .OrderBy(s => s.SpotNumber)
                .FirstOrDefault();

            if (sharedSpot != null)
                return sharedSpot.SpotNumber;

            // 2. Otherwise find a completely empty spot
            var emptySpot = _context.ParkingSpot
                .Where(s => s.VehicleId == null && s.MotorcycleCount == 0)
                .OrderBy(s => s.SpotNumber)
                .FirstOrDefault();

            return emptySpot?.SpotNumber ?? -1;
        }


        // ---------------- CAR LOGIC ----------------
        // Cars take exactly 1 spot.
        // They cannot share with motorcycles or other vehicles.
        private int FindSpotForCar()
        {
            var spot = _context.ParkingSpot
                .Where(s => s.VehicleId == null && s.MotorcycleCount == 0)
                .OrderBy(s => s.SpotNumber)
                .FirstOrDefault();

            return spot?.SpotNumber ?? -1;
        }

        // ---------------- TRUCK LOGIC ----------------
        // Trucks require 2 consecutive empty spots.
        private int FindSpotForTruck()
        {
            var spots = _context.ParkingSpot
                .OrderBy(s => s.SpotNumber)
                .ToList();

            for (int i = 0; i < spots.Count - 1; i++)
            {
                bool bothFree =
                    spots[i].VehicleId == null && spots[i].MotorcycleCount == 0 &&
                    spots[i + 1].VehicleId == null && spots[i + 1].MotorcycleCount == 0;

                if (bothFree)
                    return spots[i].SpotNumber;
            }

            return -1;
        }


        // ---------------- LARGE VEHICLE LOGIC ----------------
        // Boats and planes require 3 consecutive empty spots.
        private int FindSpotForLargeVehicle()
        {
            var spots = _context.ParkingSpot
                .OrderBy(s => s.SpotNumber)
                .ToList();

            for (int i = 0; i <= spots.Count - 3; i++) // <= instead of <
            {
                var s1 = spots[i];
                var s2 = spots[i + 1];
                var s3 = spots[i + 2];

                bool threeFree =
                    s1.VehicleId == null && s1.MotorcycleCount == 0 &&
                    s2.VehicleId == null && s2.MotorcycleCount == 0 &&
                    s3.VehicleId == null && s3.MotorcycleCount == 0;

                if (threeFree)
                    return s1.SpotNumber;
            }

            return -1;
        }

        // Updates the ParkingSpot table after a vehicle has been saved.
        // This method assigns the vehicle to one or more spots depending on its type.
        private async Task AssignSpotToParkingSpotTable(ParkedVehicle vehicle, int spot)
        {
            // Motorcycle logic: increase motorcycle count on the spot
            if (vehicle.Type == VehicleType.Motorcycle)
            {
                var s = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot);
                s.MotorcycleCount++;
                await _context.SaveChangesAsync();
                return;
            }

            // Car logic: occupies exactly 1 spot
            if (vehicle.Type == VehicleType.Car)
            {
                var s = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot);
                s.VehicleId = vehicle.Id;
                await _context.SaveChangesAsync();
                return;
            }

            // Truck logic: occupies 2 consecutive spots
            if (vehicle.Type == VehicleType.Truck)
            {
                var s1 = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot);
                var s2 = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot + 1);

                s1.VehicleId = vehicle.Id;
                s2.VehicleId = vehicle.Id;

                await _context.SaveChangesAsync();
                return;
            }

            // Boat / Plane logic: occupies 3 consecutive spots
            if (vehicle.Type == VehicleType.Boat || vehicle.Type == VehicleType.Airplane)
            {
                var s1 = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot);
                var s2 = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot + 1);
                var s3 = await _context.ParkingSpot.FirstAsync(x => x.SpotNumber == spot + 2);

                s1.VehicleId = vehicle.Id;
                s2.VehicleId = vehicle.Id;
                s3.VehicleId = vehicle.Id;

                await _context.SaveChangesAsync();
                return;
            }
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
            ViewBag.FreeSpots = await _context.ParkingSpot
                .CountAsync(s => s.VehicleId == null && s.MotorcycleCount == 0);
            ViewBag.OccupiedSpots = await _context.ParkingSpot
                .CountAsync(s => s.VehicleId != null || s.MotorcycleCount > 0);
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
                return NotFound();

            var spots = await _context.ParkingSpot
                .Where(s => s.VehicleId == parkedVehicle.Id)
                .OrderBy(s => s.SpotNumber)
                .ToListAsync();

            ViewBag.Spots = spots;

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
        // Handles the check-in process and assigns a suitable parking spot.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn([Bind("Id,RegistrationNumber,Type,Color,Brand,Model,NumberOfWheels,ArrivalTime,Note")] ParkedVehicle parkedVehicle)
        {
            // Re-populate dropdown lists
            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();

            // Validate model
            if (!ModelState.IsValid)
                return View(parkedVehicle);

            // Prevent duplicate registration numbers
            var exists = await _context.ParkedVehicle
                .AnyAsync(v => v.RegistrationNumber == parkedVehicle.RegistrationNumber);

            if (exists)
            {
                TempData["ErrorMessage"] = "Failed to enter vehicle into garage.";
                ModelState.AddModelError(nameof(ParkedVehicle.RegistrationNumber),
                    "A vehicle with this registration number already exists in the garage.");
                return View(parkedVehicle);
            }

            // Check if garage is full (simple count check)
            var occupied = await _context.ParkedVehicle.CountAsync();
            if (occupied >= _totalSpots)
            {
                TempData["ErrorMessage"] = "Failed. Garage is full.";
                return RedirectToAction(nameof(Index));
            }

            // Determine a suitable parking spot based on vehicle type
            int spot = FindAvailableSpot(parkedVehicle);

            // If no suitable spot exists
            if (spot == -1)
            {
                TempData["ErrorMessage"] = "No suitable parking spot available.";
                return RedirectToAction(nameof(Index));
            }

            // Assign the selected spot
            parkedVehicle.SpotNumber = spot;

            // Save the vehicle
            _context.Add(parkedVehicle);
            await _context.SaveChangesAsync();

            // Update the ParkingSpot table after the vehicle has an Id
            await AssignSpotToParkingSpotTable(parkedVehicle, spot);

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
                OccupiedSpots = _context.ParkingSpot
                .Where(s => s.VehicleId == pv.Id)
                .OrderBy(s => s.SpotNumber)
                .Select(s => s.SpotNumber)
                .ToList()


            });
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


        public async Task<IActionResult> Map()
        {
            var spots = await _context.ParkingSpot
                .Include(s => s.Vehicle)
                .ToListAsync();

            return View(spots);
        }


    }
}
