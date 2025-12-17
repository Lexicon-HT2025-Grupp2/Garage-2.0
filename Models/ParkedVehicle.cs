using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkedVehicle
    {
        [Required]
        [Key]
        public string RegistrationNumber { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public DateTime ArrivalTime { get; set; } = DateTime.Now;
        public string Note { get; set; }
    }
}
