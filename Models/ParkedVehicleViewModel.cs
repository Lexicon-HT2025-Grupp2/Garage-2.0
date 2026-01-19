using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkedVehicleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Vehicle Type")]
        public VehicleType? Type { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Parked Duration")]
        public TimeSpan ParkedDuration => DateTime.Now - ArrivalTime;

        [Display(Name = "Parking Spots")]
        public string ParkingSpots { get; set; }

        // Formatted duration for UI
        [Display(Name = "Parked Duration")]
        public string FormattedDuration
        {
            get
            {
                var duration = DateTime.Now - ArrivalTime;

                return $"{duration.Days}d {duration.Hours}h {duration.Minutes}m";
            }
        }
        public ApplicationUser? Owner { get; set; }

    }
}
