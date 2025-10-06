using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;

namespace TruckScheduler.Controllers
{
    public class TrucksController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TrucksController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var trucks = await _db.Trucks.Include(t => t.Driver).ToListAsync();
            return View(trucks);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Drivers = await _db.Drivers.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Truck truck)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Drivers = await _db.Drivers.ToListAsync();
                return View(truck);
            }
            _db.Trucks.Add(truck);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var t = await _db.Trucks.FindAsync(id);
            if (t == null) return NotFound();
            ViewBag.Drivers = await _db.Drivers.ToListAsync();
            return View(t);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Truck truck)
        {
            if (id != truck.Id) return BadRequest();
            if (!ModelState.IsValid) { ViewBag.Drivers = await _db.Drivers.ToListAsync(); return View(truck); }
            _db.Update(truck);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.Trucks.FindAsync(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = await _db.Trucks.FindAsync(id);
            if (t != null)
            {
                _db.Trucks.Remove(t);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
