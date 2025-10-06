using TruckScheduler.Models;

namespace TruckScheduler.Services
{
    public interface IBayAssignmentService
    {
        Task<ParkingBay?> AssignBayForCargoAsync(CargoType cargoType, CancellationToken cancellationToken = default);
        Task ReleaseBayAsync(int parkingBayId, CancellationToken cancellationToken = default);
    }
}
