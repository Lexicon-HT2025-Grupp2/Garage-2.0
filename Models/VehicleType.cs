namespace Garage_2._0.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
[PrimaryKey(nameof(Id))]
public class VehicleType
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string? Name { get; set; }
    public ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
    public ICollection<ParkingSpotType> PTypes = new List<ParkingSpotType>();
}
