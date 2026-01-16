namespace Garage_2._0.Models;

public class Parking
{
    public int Id { get; set; }

    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public required int ParkingSpotId { get; set; }
    public required ParkingSpot ParkingSpot { get; set; }

    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
}
