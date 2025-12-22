namespace Garage_2._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int SpotNumber { get; set; }
        public int MotorcycleCount { get; set; }
        public int? ParkedVehicleId { get; set; }
    }
}
