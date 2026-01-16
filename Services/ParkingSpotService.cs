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
            return new();
        }

        public string FindAvailableSpots(List<Vehicle> parkedVehicles, VehicleType vehicleType)
        {
            return "n/a";
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
