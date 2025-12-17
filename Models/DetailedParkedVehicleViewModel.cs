namespace Garage_2._0.Models
{
    public class DetailedParkedVehicleViewModel
    {
        public string RegistrationNumber { get; set; }
        public string Type { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan ParkedDuration => DateTime.Now - ArrivalTime;
    }
}
