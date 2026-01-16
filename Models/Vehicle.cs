namespace Garage_2._0.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    public int Id { get; set; }
    [Required, MaxLength(10)]
    [Display(Name = "Registration Number")]
    public string RegistrationNumber { get; set; } = "";
    [Display(Name = "Vehicle Type")]
    public int VehicleTypeId { get; set; }
    public VehicleType? VehicleType { get; set; }
    [MaxLength(30)] public string Color { get; set; } = "";
    [MaxLength(50)] public string Brand { get; set; } = "";
    [MaxLength(50)] public string Model { get; set; } = "";
    [Display(Name = "Number Of Wheels")]
    public int NumberOfWheels { get; set; }
    [Display(Name = "Arrival Time")]
    public DateTime ArrivalTime { get; set; } = DateTime.Now;

    [Display(Name = "Note")]
    [Required(ErrorMessage = "Note is required")]
    public string Note { get; set; } = string.Empty;
    [Display(Name = "Parking Spots")]
    public string? ParkingSpots { get; set; }
    public ApplicationUser? Owner { get; set; }
    [ValidateNever]
    public string OwnerId { get; set; }

    public string? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }


}
