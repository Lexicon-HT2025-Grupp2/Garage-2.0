using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Models
{
    [PrimaryKey(nameof(Id))]
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int? ParentId {  get; set; }  // overlay, parent.
        public ParkingSpot? Parent { get; set; }
        public string? Name { get; set; }
        public ICollection<int> VehicleTypeIds { get; set; }
        public ICollection<ParkingSpot> SubSpots { get; set; } = new List<ParkingSpot>();  // included spots, children.
    }
}
