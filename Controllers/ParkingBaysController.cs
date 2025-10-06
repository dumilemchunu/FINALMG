using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;

namespace TruckScheduler.Controllers
{
    public class ParkingBaysController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly List<CargoType> _priorityOrder = new List<CargoType>
        {
            CargoType.Hazardous,
            CargoType.Refrigerated,
            CargoType.Perishable,
            CargoType.Fragile,
            CargoType.Oversized,
            CargoType.General
        };

        public ParkingBaysController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var bays = await _db.ParkingBays.ToListAsync();
            var ordered = bays.OrderBy(b => _priorityOrder.IndexOf(b.AllowedCargoType)).ThenBy(b => b.BayNumber).ToList();
            return View(ordered);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ParkingBay bay)
        {
            if (!ModelState.IsValid) return View(bay);
            _db.ParkingBays.Add(bay);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var b = await _db.ParkingBays.FindAsync(id);
            if (b == null) return NotFound();
            return View(b);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ParkingBay bay)
        {
            if (id != bay.Id) return BadRequest();
            if (!ModelState.IsValid) return View(bay);
            _db.Update(bay);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
