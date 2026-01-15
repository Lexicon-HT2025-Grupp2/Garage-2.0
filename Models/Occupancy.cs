namespace Garage_2._0.Models
{
    public class Occupancy
    {
        public required int ParkingSpotId { get; set; }
        public List<int> VehicleIds { get; set; } = new();
        public required ParkingSpot ParkingSpot { get; set; }
    }
}
