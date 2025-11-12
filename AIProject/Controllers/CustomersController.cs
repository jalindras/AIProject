using System.Linq;
using System.Threading.Tasks;
using AIProject.Data;
using AIProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Customers
                .AsNoTracking()
                .Include(c => c.GenderOption)
                .Include(c => c.PreferredContactMethodOption)
                .Include(c => c.PreferredLanguageOption);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(c =>
                    (c.FirstName + " " + c.LastName).Contains(search) ||
                    (c.Email ?? string.Empty).Contains(search) ||
                    (c.PhoneNumber ?? string.Empty).Contains(search));
            }

            var customers = await query
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();

            ViewData["Search"] = search;
            return View(customers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.GenderOption)
                .Include(c => c.PreferredContactMethodOption)
                .Include(c => c.PreferredLanguageOption)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateSelectListsAsync();
            return View(new Customer
            {
                MarketingOptIn = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return View(customer);
            }

            _context.Add(customer);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Customer has been registered successfully.";
            return RedirectToAction(nameof(Details), new { id = customer.Id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await PopulateSelectListsAsync();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateSelectListsAsync();
                return View(customer);
            }

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Customer information has been updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Details), new { id = customer.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Customer profile has been removed.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        private async Task PopulateSelectListsAsync()
        {
            ViewData["GenderOptions"] = await _context.GenderOptions
                .AsNoTracking()
                .OrderBy(option => option.Name)
                .Select(option => new SelectListItem
                {
                    Text = option.Name,
                    Value = option.Id.ToString()
                })
                .ToListAsync();

            ViewData["PreferredContactMethodOptions"] = await _context.PreferredContactMethodOptions
                .AsNoTracking()
                .OrderBy(option => option.Name)
                .Select(option => new SelectListItem
                {
                    Text = option.Name,
                    Value = option.Id.ToString()
                })
                .ToListAsync();

            ViewData["PreferredLanguageOptions"] = await _context.PreferredLanguageOptions
                .AsNoTracking()
                .OrderBy(option => option.Name)
                .Select(option => new SelectListItem
                {
                    Text = option.Name,
                    Value = option.Id.ToString()
                })
                .ToListAsync();
        }
    }
}
