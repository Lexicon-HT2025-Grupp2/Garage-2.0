using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Models
{
    [PrimaryKey(nameof(Id))]
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int? Parent {  get; set; }
        public string? Name { get; set; }
        public ICollection<VehicleType> VehicleTypes { get; set; }
    }
}
