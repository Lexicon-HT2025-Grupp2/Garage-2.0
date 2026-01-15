using Garage_2._0.Models;

namespace Garage_2._0.Services
{
    public class ParkingSpotService
    {
        public const int TotalSpots = 50; // Total parking spots in the garage

        public class SpotState
        {
            public int SpotNumber { get; set; }
            public bool IsOccupied { get; set; }
            public int MotorcycleCount { get; set; } // 0-3 motorcycles per spot
            public List<string> VehicleRegistrations { get; set; } = new List<string>();
            public VehicleType? OccupiedBy { get; set; }
        }

        public List<SpotState> GetParkingSpotStates(List<Vehicle> parkedVehicles)
        {
            var spots = Enumerable.Range(1, TotalSpots)
                .Select(i => new SpotState { SpotNumber = i })
                .ToList();

            foreach (var vehicle in parkedVehicles)
            {
                if (string.IsNullOrEmpty(vehicle.ParkingSpots))
                    continue;

                var spotNumbers = vehicle.ParkingSpots.Split(',').Select(s => s.Trim()).ToList();

                if (vehicle.VehicleType != null && vehicle.VehicleType.Name == "Motorcycle")
                {
                    if (spotNumbers.Count >= 1 && int.TryParse(spotNumbers[0], out int spotNum))
                    {
                        var spot = spots.FirstOrDefault(s => s.SpotNumber == spotNum);
                        if (spot != null)
                        {
                            spot.MotorcycleCount++;
                            spot.VehicleRegistrations.Add(vehicle.RegistrationNumber);
                            spot.OccupiedBy = vehicle.VehicleType;
                            spot.IsOccupied = spot.MotorcycleCount >= 3;
                        }
                    }
                }
                else
                {
                    // Regular vehicles
                    foreach (var spotStr in spotNumbers)
                    {
                        if (int.TryParse(spotStr, out int spotNum))
                        {
                            var spot = spots.FirstOrDefault(s => s.SpotNumber == spotNum);
                            if (spot != null)
                            {
                                spot.IsOccupied = true;
                                spot.VehicleRegistrations.Add(vehicle.RegistrationNumber);
                                spot.OccupiedBy = vehicle.VehicleType;
                            }
                        }
                    }
                }
            }

            return spots;
        }

        public string FindAvailableSpots(List<Vehicle> parkedVehicles, VehicleType vehicleType)
        {
            var spots = GetParkingSpotStates(parkedVehicles);

            // Fix: Compare using VehicleType.Name or use a VehicleType enum/constant if available
            if (vehicleType != null && vehicleType.Name == "Motorcycle")
            {
                var partialSpot = spots.FirstOrDefault(s =>
                    s.MotorcycleCount > 0 && s.MotorcycleCount < 3);

                if (partialSpot != null)
                {
                    char slot = partialSpot.MotorcycleCount == 1 ? 'B' : 'C';
                    return $"{partialSpot.SpotNumber},{slot}";
                }

                var freeSpot = spots.FirstOrDefault(s => !s.IsOccupied && s.MotorcycleCount == 0);
                if (freeSpot != null)
                {
                    return $"{freeSpot.SpotNumber},A";
                }

                return null;
            }

            int requiredSpots = vehicleType != null ? vehicleType.Name switch
            {
                "Truck" => 2,
                "Boat" => 3,
                "Airplane" => 3,
                _ => 1
            } : 1;

            return FindConsecutiveSpots(spots, requiredSpots);
        }

        private string FindConsecutiveSpots(List<SpotState> spots, int count)
        {
            for (int i = 0; i <= spots.Count - count; i++)
            {
                bool allFree = true;
                for (int j = 0; j < count; j++)
                {
                    var spot = spots[i + j];
                    if (spot.IsOccupied || spot.MotorcycleCount > 0)
                    {
                        allFree = false;
                        break;
                    }
                }

                if (allFree)
                {
                    var spotNumbers = Enumerable.Range(spots[i].SpotNumber, count)
                        .Select(n => n.ToString());
                    return string.Join(",", spotNumbers);
                }
            }

            return null;
        }

        public bool CanParkVehicleType(List<Vehicle> parkedVehicles, VehicleType vehicleType)
        {
            return FindAvailableSpots(parkedVehicles, vehicleType) != null;
        }

        public int GetAvailableSpotCount(List<Vehicle> parkedVehicles)
        {
            var spots = GetParkingSpotStates(parkedVehicles);
            return spots.Count(s => !s.IsOccupied && s.MotorcycleCount == 0);
        }

        public int GetOccupiedSpotCount(List<Vehicle> parkedVehicles)
        {
            var spots = GetParkingSpotStates(parkedVehicles);
            return spots.Count(s => s.IsOccupied || s.MotorcycleCount > 0);
        }
    }
}
