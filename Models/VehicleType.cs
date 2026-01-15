namespace Garage_2._0.Models;

using System.ComponentModel.DataAnnotations;
public class VehicleType
{
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Name { get; set; } = "";
}
