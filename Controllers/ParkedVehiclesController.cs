using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    public class ParkedVehiclesController : Controller
    {
        private readonly Garage_2_0Context _context;
        private readonly PricingService _pricingService;
        private readonly ParkingSpotService _parkingSpotService;

        public ParkedVehiclesController(Garage_2_0Context context, PricingService pricing)
        {
            _context = context;
            _pricingService = pricing;
            _parkingSpotService = new ParkingSpotService();
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



        // GET: ParkedVehicles
        //public async Task<IActionResult> Index()
        //{
        //    return View(await ParkedVehiclesQuery().ToListAsync());
        //}
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["TypeSortParm"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["RegNoSortParm"] = sortOrder == "RegNo" ? "regno_desc" : "RegNo";
            ViewData["ArrivalSortParm"] = sortOrder == "Arrival" ? "arrival_desc" : "Arrival";
            ViewData["DurationSortParm"] = sortOrder == "Duration" ? "duration_desc" : "Duration";

            var vehicles = from v in _context.ParkedVehicle
                           select v;

            // Search functionality
            if (!string.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.RegistrationNumber.Contains(searchString) ||
                                              v.Brand.Contains(searchString) ||
                                              v.Model.Contains(searchString) ||
                                              v.Color.Contains(searchString));
            }

            // Convert to list before sorting to calculate parked duration
            var vehicleList = await vehicles.ToListAsync();
            var now = DateTime.Now;

            var viewModels = vehicleList.Select(v => new ParkedVehicleViewModel
            {
                Id = v.Id,
                Type = v.Type,
                RegistrationNumber = v.RegistrationNumber,
                ArrivalTime = v.ArrivalTime,
            }).AsQueryable();

            // Sorting
            viewModels = sortOrder switch
            {
                "RegNo" => viewModels.OrderBy(v => v.RegistrationNumber),
                "regno_desc" => viewModels.OrderByDescending(v => v.RegistrationNumber),
                "type" => viewModels.OrderBy(v => v.Type),
                "type_desc" => viewModels.OrderByDescending(v => v.Type),
                "Arrival" => viewModels.OrderBy(v => v.ArrivalTime),
                "Duration" => viewModels.OrderBy(v => v.ParkedDuration),
                "duration_desc" => viewModels.OrderByDescending(v => v.ParkedDuration),
                _ => viewModels.OrderByDescending(v => v.ArrivalTime)
            };

            return View(viewModels.ToList());
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
        public async Task<IActionResult> CheckIn([Bind("Id,RegistrationNumber,Type,Color,Brand,Model,NumberOfWheels,Note")] ParkedVehicle parkedVehicle)
        {
            var parkedVehicles = await _context.ParkedVehicle.ToListAsync();

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

            ModelState.Remove("ParkingSpots");

            // Check if parking spots are available for this vehicle type
            var availableSpots = _parkingSpotService.FindAvailableSpots(parkedVehicles, parkedVehicle.Type);
            if (availableSpots == null)
            {
                ModelState.AddModelError("VehicleType", $"No available parking spots for {parkedVehicle.Type}. Please try another vehicle type.");
            }
            else
            {
                parkedVehicle.ParkingSpots = availableSpots;
            }

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
        public async Task<IActionResult> Edit(int id, [
            Bind("Id,RegistrationNumber,Type,Color,Brand,Model,NumberOfWheels,Note")]
            ParkedVehicle parkedVehicle)
        {
            // Vet inte om denna check ens behövs då id och parkedVehicle.Id kommer väl ifrån samma källa?
            if (id != parkedVehicle.Id)
            {
                return NotFound($"Unable to find vehicle with id: {parkedVehicle.Id}, in database.");
            }

            ViewBag.VehicleTypes = MakeEnumList<VehicleType>();
            ViewBag.Colors = MakeEnumList<ConsoleColor>();

            var dbParkedVehicle = await _context.ParkedVehicle.FindAsync(parkedVehicle.Id);

            if (dbParkedVehicle == null)
            {

                return NotFound($"Failed to retrieve vehicle with id: {parkedVehicle.Id}, from database.");
            }

            if (dbParkedVehicle.Id != parkedVehicle.Id && dbParkedVehicle.RegistrationNumber == parkedVehicle.RegistrationNumber)
            {
                TempData["ErrorMessage"] = "Failed to update parked vehicle.";

                ModelState.AddModelError(nameof(ParkedVehicle.RegistrationNumber),
                    "Vehicle already exists in the garage.");

                return View(parkedVehicle);
            }

            if (!ModelState.IsValid)
            {
                return View(parkedVehicle);
            }

            try
            {
                dbParkedVehicle.RegistrationNumber = parkedVehicle.RegistrationNumber;
                dbParkedVehicle.Type = parkedVehicle.Type;
                dbParkedVehicle.Color = parkedVehicle.Color;
                dbParkedVehicle.Brand = parkedVehicle.Brand;
                dbParkedVehicle.Model = parkedVehicle.Model;
                dbParkedVehicle.NumberOfWheels = parkedVehicle.NumberOfWheels;
                dbParkedVehicle.Note = parkedVehicle.Note;

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
                ArrivalTime = pv.ArrivalTime
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

        private GarageStatisticsViewModel CalculateStatistics(List<ParkedVehicle> vehicles)
        {
            var now = DateTime.Now;
            var stats = new GarageStatisticsViewModel
            {
                TotalVehicles = vehicles.Count,
                VehiclesByType = new Dictionary<VehicleType, int>(),
                TotalWheels = vehicles.Sum(v => v.NumberOfWheels),
                TotalRevenue = 0,
                TotalSpots = ParkingSpotService.TotalSpots,
                OccupiedSpots = _parkingSpotService.GetOccupiedSpotCount(vehicles),
                AvailableSpots = _parkingSpotService.GetAvailableSpotCount(vehicles),
                AverageParkingDurationHours = vehicles.Any()
                    ? vehicles.Average(v => (now - v.ArrivalTime).TotalHours)
                    : 0
            };

            // Count vehicles by type
            foreach (VehicleType type in Enum.GetValues(typeof(VehicleType)))
            {
                stats.VehiclesByType[type] = vehicles.Count(v => v.Type == type);
            }

            // Calculate total revenue
            foreach (var vehicle in vehicles)
            {
                var parkingPeriod = now - vehicle.ArrivalTime;
                stats.TotalRevenue += _pricingService.CalculatePrice(vehicle.ArrivalTime, now);
            }

            // Find longest parked vehicle
            stats.LongestParkedVehicle = vehicles
                .OrderBy(v => v.ArrivalTime)
                .FirstOrDefault();

            return stats;
        }

        // GET: ParkedVehicles/ParkingMap
        public async Task<IActionResult> ParkingMap()
        {
            var vehicles = await _context.ParkedVehicle.ToListAsync();
            var spotStates = _parkingSpotService.GetParkingSpotStates(vehicles);

            ViewBag.TotalSpots = ParkingSpotService.TotalSpots;
            ViewBag.Statistics = CalculateStatistics(vehicles);

            return View(spotStates);
        }
    }
}
