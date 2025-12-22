using Garage_2._0.Data;
using Garage_2._0.Models;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Services
{
    public class ParkingService
    {
        private readonly Garage_2_0Context _context;

        public ParkingService(Garage_2_0Context context)
        {
            _context = context;
        }

        public async Task<int> ParkVehicleAsync(ParkedVehicle vehicle)
        {
            // 1. Determine how many spots the vehicle needs
            int requiredSpots = GetRequiredSpots(vehicle.Type);

            // 2. Find available spot(s)
            int startSpot = await FindAvailableSpotBlockAsync(vehicle.Type);

            // 3. Assign the vehicle to the spots
            await AssignSpotsAsync(vehicle, startSpot, requiredSpots);

            return startSpot;
        }

        private int GetRequiredSpots(VehicleType type)
        {
            return type switch
            {
                VehicleType.Motorcycle => 1, // but shared
                VehicleType.Truck => 2,
                VehicleType.Boat => 3,
                _ => 1
            };
        }

        private async Task<int> FindAvailableSpotBlockAsync(VehicleType type)
        {
            int requiredSpots = GetRequiredSpots(type);
            var spots = await _context.ParkingSpot
                .OrderBy(s => s.SpotNumber)
                .ToListAsync();
            for (int i = 0; i <= spots.Count - requiredSpots; i++)
            {
                bool blockAvailable = true;
                for (int j = 0; j < requiredSpots; j++)
                {
                    if (spots[i + j].VehicleId != null ||
                        (type == VehicleType.Motorcycle && spots[i + j].MotorcycleCount >= 3))
                    {
                        blockAvailable = false;
                        break;
                    }
                }
                if (blockAvailable)
                {
                    return spots[i].SpotNumber;
                }
            }
            throw new Exception("No available parking spots found.");
        }

        private async Task AssignSpotsAsync(ParkedVehicle vehicle, int startSpot, int requiredSpots)
        {
            vehicle.SpotNumber = startSpot;
            _context.ParkedVehicle.Add(vehicle);
            await _context.SaveChangesAsync();
            for (int i = 0; i < requiredSpots; i++)
            {
                var spot = await _context.ParkingSpot
                    .FirstAsync(s => s.SpotNumber == startSpot + i);
                if (vehicle.Type == VehicleType.Motorcycle)
                {
                    spot.MotorcycleCount += 1;
                }
                else
                {
                    spot.VehicleId = vehicle.Id;
                }
                _context.ParkingSpot.Update(spot);
            }
            await _context.SaveChangesAsync();
        }

    }
}
