using System.Linq;
using System.Threading.Tasks;
using AIProject.Data;
using AIProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Controllers
{
    public class GaugesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GaugesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Gauges
                .Include(g => g.Customer)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(g =>
                    g.SerialNumber.Contains(search) ||
                    (g.Customer != null &&
                        ((g.Customer.FirstName + " " + g.Customer.LastName).Contains(search) ||
                         (g.Customer.CompanyName ?? string.Empty).Contains(search))));
            }

            var gauges = await query
                .OrderBy(g => g.SerialNumber)
                .ToListAsync();

            ViewData["Search"] = search;
            return View(gauges);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gauge = await _context.Gauges
                .Include(g => g.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gauge == null)
            {
                return NotFound();
            }

            return View(gauge);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCustomersDropDownList();
            return View(new Gauge());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gauge gauge)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCustomersDropDownList(gauge.CustomerId);
                return View(gauge);
            }

            _context.Add(gauge);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Gauge has been created.";
            return RedirectToAction(nameof(Details), new { id = gauge.Id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gauge = await _context.Gauges.FindAsync(id);
            if (gauge == null)
            {
                return NotFound();
            }

            await PopulateCustomersDropDownList(gauge.CustomerId);
            return View(gauge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gauge gauge)
        {
            if (id != gauge.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateCustomersDropDownList(gauge.CustomerId);
                return View(gauge);
            }

            try
            {
                _context.Update(gauge);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Gauge has been updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GaugeExists(gauge.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Details), new { id = gauge.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gauge = await _context.Gauges
                .Include(g => g.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gauge == null)
            {
                return NotFound();
            }

            return View(gauge);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gauge = await _context.Gauges.FindAsync(id);
            if (gauge != null)
            {
                _context.Gauges.Remove(gauge);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Gauge has been removed.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateCustomersDropDownList(object? selectedCustomer = null)
        {
            var customers = await _context.Customers
                .AsNoTracking()
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .Select(c => new
                {
                    c.Id,
                    Name = (c.CompanyName != null && c.CompanyName != string.Empty)
                        ? $"{c.CompanyName} ({c.FirstName} {c.LastName})"
                        : $"{c.FirstName} {c.LastName}"
                })
                .ToListAsync();

            ViewData["CustomerId"] = new SelectList(customers, "Id", "Name", selectedCustomer);
        }

        private bool GaugeExists(int id)
        {
            return _context.Gauges.Any(e => e.Id == id);
        }
    }
}
