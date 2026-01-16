namespace Garage_2._0.Models;

using System.ComponentModel.DataAnnotations;

public class Vehicle
{
    public int Id { get; set; }

    [Required, MaxLength(10)]
    public string RegistrationNumber { get; set; } = "";

    public required int VehicleTypeId { get; set; }
    public required VehicleType VehicleType { get; set; }


    [MaxLength(30)] public string Color { get; set; } = "";
    [MaxLength(50)] public string Brand { get; set; } = "";
    [MaxLength(50)] public string Model { get; set; } = "";
    public int NumberOfWheels { get; set; }
    [Display(Name = "Arrival Time")]
    public DateTime ArrivalTime { get; set; } = DateTime.Now;

    public string? Note { get; set; }
    public ParkingSpot? ParkingSpot { get; set; }
    public string ParkingSpotName()
    {
        if (this.ParkingSpot == null) return "N/A";
        ParkingSpot p = this.ParkingSpot!;
        if (p.Name == null) return "(null)";
        string name = p.Name!;
        return name;
    }
    public string? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }

}
