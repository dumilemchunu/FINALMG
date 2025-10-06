using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;
using TruckScheduler.Services;

namespace TruckScheduler.Controllers
{
    public class CheckInsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBayAssignmentService _bayService;

        public CheckInsController(ApplicationDbContext db, IBayAssignmentService bayService)
        {
            _db = db;
            _bayService = bayService;
        }

        // active check-ins
        public async Task<IActionResult> Index()
        {
            var list = await _db.CheckIns
                .Include(c => c.Truck)
                .Include(c => c.Driver)
                .Include(c => c.ParkingBay)
                .Where(c => c.CheckOutTime == null)
                .ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Trucks = await _db.Trucks.Include(t => t.Driver).ToListAsync();
            ViewBag.Drivers = await _db.Drivers.ToListAsync();
            ViewBag.CargoTypes = Enum.GetValues(typeof(CargoType)).Cast<CargoType>().ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int truckId, int? driverId, CargoType cargoType, string cargoDescription)
        {
            var truck = await _db.Trucks.FindAsync(truckId);
            if (truck == null) { ModelState.AddModelError("", "Truck must be selected."); }

            if (!ModelState.IsValid)
            {
                ViewBag.Trucks = await _db.Trucks.Include(t => t.Driver).ToListAsync();
                ViewBag.Drivers = await _db.Drivers.ToListAsync();
                ViewBag.CargoTypes = Enum.GetValues(typeof(CargoType)).Cast<CargoType>().ToList();
                return View();
            }

            // assign bay
            var bay = await _bayService.AssignBayForCargoAsync(cargoType);
            var checkin = new CheckIn
            {
                TruckId = truckId,
                DriverId = driverId,
                CargoType = cargoType,
                CargoDescription = cargoDescription,
                CheckInTime = DateTime.UtcNow,
                ParkingBayId = bay?.Id
            };

            _db.CheckIns.Add(checkin);
            await _db.SaveChangesAsync();

            TempData["Message"] = bay != null
                ? $"Truck assigned to bay {bay.BayNumber} (allowed: {bay.AllowedCargoType})."
                : "No bay available at the moment. Check-in recorded with no assigned bay.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var c = await _db.CheckIns.Include(x => x.Truck).Include(x => x.Driver).Include(x => x.ParkingBay).FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(int id)
        {
            var checkin = await _db.CheckIns.FindAsync(id);
            if (checkin == null) return NotFound();
            checkin.CheckOutTime = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            await _bayService.ReleaseBayAsync(checkin.ParkingBayId ?? 0);
            return RedirectToAction(nameof(Index));
        }

        // history (past checkins)
        public async Task<IActionResult> History()
        {
            var list = await _db.CheckIns.Include(c => c.Truck).Include(c => c.ParkingBay).Where(c => c.CheckOutTime != null).OrderByDescending(c => c.CheckOutTime).ToListAsync();
            return View(list);
        }
    }
}
