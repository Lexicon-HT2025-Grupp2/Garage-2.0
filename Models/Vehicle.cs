namespace Garage_2._0.Models;

using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string RegistrationNumber { get; set; } = "";

    public int VehicleTypeId { get; set; }
    public VehicleType? VehicleType { get; set; }


    [MaxLength(30)] public string Color { get; set; } = "";
    [MaxLength(50)] public string Brand { get; set; } = "";
    [MaxLength(50)] public string Model { get; set; } = "";
    public int NumberOfWheels { get; set; }
    [Display(Name = "Arrival Time")]
    public DateTime ArrivalTime { get; set; } = DateTime.Now;

    [Display(Name = "Note")]
    [Required(ErrorMessage = "Note is required")]
    public string Note { get; set; } = string.Empty;
    [Display(Name = "Parking Spots")]
    public string? ParkingSpots { get; set; }

    public string GetFormattedParkingSpots()
    {
        if (string.IsNullOrEmpty(ParkingSpots))
            return "N/A";

        if (VehicleType != null && VehicleType.Name == "Motorcycle")
        {
            // For motorcycles, show which slot (A, B, or C)
            var spots = ParkingSpots.Split(',');
            if (spots.Length == 2)
            {
                var spotNumber = spots[0];
                var slot = spots[1];
                return $"{spotNumber}-{slot}";
            }
        }

        return ParkingSpots.Replace(",", ", ");
    }

    public string? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }


}
