using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;

namespace TruckScheduler.Services
{
    public class BayAssignmentService : IBayAssignmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly List<CargoType> _priority = new List<CargoType>
        {
            CargoType.Hazardous,
            CargoType.Refrigerated,
            CargoType.Perishable,
            CargoType.Fragile,
            CargoType.Oversized,
            CargoType.General
        };

        public BayAssignmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ParkingBay?> AssignBayForCargoAsync(CargoType cargoType, CancellationToken cancellationToken = default)
        {
            // get occupied bay ids (active checkins)
            var occupiedBayIds = await _db.CheckIns
                .Where(c => c.CheckOutTime == null && c.ParkingBayId != null)
                .Select(c => c.ParkingBayId!.Value)
                .ToListAsync(cancellationToken);

            // 1) exact match
            var bay = await _db.ParkingBays
                .Where(b => b.AllowedCargoType == cargoType && !occupiedBayIds.Contains(b.Id))
                .OrderBy(b => b.BayNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (bay != null) return bay;

            // 2) try other priority-friendly bays (use priority list but prefer general later)
            foreach (var ct in _priority)
            {
                if (ct == cargoType) continue; // already tried
                var b = await _db.ParkingBays
                    .Where(x => x.AllowedCargoType == ct && !occupiedBayIds.Contains(x.Id))
                    .OrderBy(x => x.BayNumber)
                    .FirstOrDefaultAsync(cancellationToken);
                if (b != null) return b;
            }

            // 3) fallback: any free bay
            var any = await _db.ParkingBays
                .Where(b => !occupiedBayIds.Contains(b.Id))
                .OrderBy(b => b.BayNumber)
                .FirstOrDefaultAsync(cancellationToken);

            return any;
        }

        public Task ReleaseBayAsync(int parkingBayId, CancellationToken cancellationToken = default)
        {
            // Nothing in DB needs to be done here - check-ins store checkOutTime. This method exists for future hooks.
            return Task.CompletedTask;
        }
    }
}
