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

        public List<SpotState> GetParkingSpotStates(List<ParkedVehicle> parkedVehicles)
        {
            var spots = Enumerable.Range(1, TotalSpots)
                .Select(i => new SpotState { SpotNumber = i })
                .ToList();

            foreach (var vehicle in parkedVehicles)
            {
                if (string.IsNullOrEmpty(vehicle.ParkingSpots))
                    continue;

                var spotNumbers = vehicle.ParkingSpots.Split(',').Select(s => s.Trim()).ToList();

                if (vehicle.Type == VehicleType.Motorcycle)
                {
                    if (spotNumbers.Count >= 1 && int.TryParse(spotNumbers[0], out int spotNum))
                    {
                        var spot = spots.FirstOrDefault(s => s.SpotNumber == spotNum);
                        if (spot != null)
                        {
                            spot.MotorcycleCount++;
                            spot.VehicleRegistrations.Add(vehicle.RegistrationNumber);
                            spot.OccupiedBy = VehicleType.Motorcycle;
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
                                spot.OccupiedBy = vehicle.Type;
                            }
                        }
                    }
                }
            }

            return spots;
        }

        public string FindAvailableSpots(List<ParkedVehicle> parkedVehicles, VehicleType vehicleType)
        {
            var spots = GetParkingSpotStates(parkedVehicles);

            if (vehicleType == VehicleType.Motorcycle)
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

            int requiredSpots = vehicleType switch
            {
                VehicleType.Truck => 2,
                VehicleType.Boat => 3,
                VehicleType.Airplane => 3,
                _ => 1
            };

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

        public bool CanParkVehicleType(List<ParkedVehicle> parkedVehicles, VehicleType vehicleType)
        {
            return FindAvailableSpots(parkedVehicles, vehicleType) != null;
        }

        public int GetAvailableSpotCount(List<ParkedVehicle> parkedVehicles)
        {
            var spots = GetParkingSpotStates(parkedVehicles);
            return spots.Count(s => !s.IsOccupied && s.MotorcycleCount == 0);
        }

        public int GetOccupiedSpotCount(List<ParkedVehicle> parkedVehicles)
        {
            var spots = GetParkingSpotStates(parkedVehicles);
            return spots.Count(s => s.IsOccupied || s.MotorcycleCount > 0);
        }
    }
}
