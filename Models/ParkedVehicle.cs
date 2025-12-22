using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public enum VehicleType
    {
        Car,
        Motorcycle,
        Bus,
        Bike,
        Boat,
        Airplane,
        Truck,
        Trailer
    }

    public class ParkedVehicle
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Registration number is required")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Registration number must be between 3 and 10 characters")]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        [Display(Name = "Vehicle Type")]
        public VehicleType Type { get; set; }

        [Required(ErrorMessage = "Color is required")]
        [StringLength(30, ErrorMessage = "Color cannot exceed 30 characters")]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        [StringLength(50, ErrorMessage = "Brand cannot exceed 50 characters")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9][A-Za-z0-9\s\-]*$",
            ErrorMessage = "Model cannot start with a minus sign")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Number of wheels is required")]
        [Range(0, 20, ErrorMessage = "Number of wheels must be between 0 and 20")]
        [Display(Name = "Number of Wheels")]
        public int NumberOfWheels { get; set; }

        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; } = DateTime.Now;

        [Display(Name = "Note")]
        [Required(ErrorMessage = "Note is required")]
        public string Note { get; set; } = string.Empty;
        // The first spot of the vehicle (for multi-spot vehicles)
        // All other spots will reference the same VehicleId in ParkingSpot
        [Display(Name = "Parking Spot")]
        public int SpotNumber { get; set; }

        public int RequiredSpots =>
            Type switch
            {
                VehicleType.Motorcycle => 1, 
                VehicleType.Car => 1,
                VehicleType.Truck => 2,
                VehicleType.Boat => 3,
                VehicleType.Airplane  => 3,
                _=> 1
            };

            }
}
