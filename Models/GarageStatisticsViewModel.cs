namespace Garage_2._0.Models
{
    public class GarageStatisticsViewModel
    {
        public int TotalVehicles { get; set; }
        public int TotalWheels { get; set; }
        public Dictionary<VehicleType, int> VehiclesByType { get; set; } = new();
        public Dictionary<string, int> VehiclesByColor { get; set; } = new();
        public double AverageParkingDurationHours { get; set; }
        public double TotalRevenue { get; set; }
        public Dictionary<VehicleType, double> RevenueByType { get; set; } = new();
        public int AvailableSpots { get; set; }
        public int OccupiedSpots { get; set; }
        public int TotalSpots { get; set; }
        public Vehicle LongestParkedVehicle { get; set; }
    }
}
