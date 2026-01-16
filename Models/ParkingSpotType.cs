namespace Garage_2._0.Models
{
    public class ParkingSpotType
    {
        public required int ParkingSpotId { get; set; }
        public required ParkingSpot ParkingSpot { get; set; }
        public required int VehicleTypeId { get; set; }
        public required VehicleType VehicleType { get; set; }
    }
}
