namespace Garage_2._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }

        // The actual spot number
        public int SpotNumber { get; set; }

        // If a large vehicle occupies multiple spots,
        // each spot will reference the same VehicleId
        public int? VehicleId { get; set; }
        public ParkedVehicle Vehicle { get; set; }

        // For motorcycles: 0–3
        public int MotorcycleCount { get; set; }


    }
}
