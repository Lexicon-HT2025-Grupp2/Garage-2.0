namespace Garage_2._0.Models
{
    public class ReceiptViewModel
    {
        public string RegistrationNumber { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public double Price { get; set; }

    }
}
