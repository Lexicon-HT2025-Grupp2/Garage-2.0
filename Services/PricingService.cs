namespace Garage_2._0.Services
{
    public class PricingService
    {
        private const double HourlyRate = 10.0;

        public double CalculatePrice(DateTime arrivalTime, DateTime departureTime)
        {
            var totalHours = (departureTime - arrivalTime).TotalHours;
            return Math.Round(totalHours * HourlyRate, 2);
        }
    }

}
