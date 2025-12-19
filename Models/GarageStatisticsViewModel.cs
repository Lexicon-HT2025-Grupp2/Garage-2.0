namespace Garage_2._0.Models
{
    public class GarageStatisticsViewModel
    {
        public int TotalVehicles { get; set; }
        public int TotalWheels { get; set; }
        public Dictionary<VehicleType, int> VehiclesByType { get; set; } = new();
        public Dictionary<string, int> VehiclesByColor { get; set; } = new();
        public double TotalRevenue { get; set; }

    }
}
