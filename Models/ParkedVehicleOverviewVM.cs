namespace Garage_2._0.Models
{
    public class ParkedVehicleOverviewVM
    {
        public int ParkingId { get; set; }

        public string OwnerName { get; set; } = string.Empty;

        public string VehicleType { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;

        public int SpotNumber { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan ParkingTime => DateTime.Now - StartTime;
    }
}