using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruckScheduler.Data;
using TruckScheduler.Models;

namespace TruckScheduler.Controllers
{
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DriversController(ApplicationDbContext db) { _db = db; }

        public async Task<IActionResult> Index() => View(await _db.Drivers.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Driver driver)
        {
            if (!ModelState.IsValid) return View(driver);
            _db.Drivers.Add(driver);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Driver driver)
        {
            if (id != driver.Id) return BadRequest();
            if (!ModelState.IsValid) return View(driver);
            _db.Update(driver);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d == null) return NotFound();
            return View(d);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var d = await _db.Drivers.FindAsync(id);
            if (d != null) { _db.Drivers.Remove(d); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
