namespace Garage_2._0.Models
{
    public class ParkingSpotType
    {
        public required int ParkingSpotId { get; set; }
        public required ParkingSpot Spot { get; set; }
        public int? VehicleTypeId { get; set; } = 0;
    }
}
