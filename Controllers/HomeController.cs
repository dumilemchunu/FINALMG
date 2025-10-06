using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;
namespace TruckScheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var totalTrucks = await _db.Trucks.CountAsync();
            var totalDrivers = await _db.Drivers.CountAsync();
            var totalBays = await _db.ParkingBays.CountAsync();
            var occupiedBays = await _db.CheckIns.CountAsync(c => c.CheckOutTime == null && c.ParkingBayId != null);
            var activeCheckIns = await _db.CheckIns.Include(c => c.Truck).Include(c => c.ParkingBay).Where(c => c.CheckOutTime == null).ToListAsync();

            var model = new
            {
                TotalTrucks = totalTrucks,
                TotalDrivers = totalDrivers,
                TotalBays = totalBays,
                OccupiedBays = occupiedBays,
                ActiveCheckIns = activeCheckIns
            };

            return View(model);
        }
    }
}
