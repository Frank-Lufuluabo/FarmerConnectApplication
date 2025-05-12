using System;
using System.Linq;
using System.Threading.Tasks;
using FarmerConnectApplication.Data;
using FarmerConnectApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FarmerConnectApplication.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                if (farmer != null)
                {
                    query = query.Where(p => p.FarmerId == farmer.Id);
                }
            }

            return View(await query.ToListAsync());
        }

        [Authorize(Roles = "Farmer,Employee")]
        public IActionResult Create()
        {
            if (User.IsInRole("Employee"))
            {
                ViewBag.Farmers = _context.Farmers
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                    .ToList();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Farmer"))
                {
                    var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                    if (farmer == null)
                    {
                        ModelState.AddModelError("", "Farmer profile not found.");
                        return View(product);
                    }
                    product.FarmerId = farmer.Id;
                }
                else if (User.IsInRole("Employee"))
                {
                    var farmer = await _context.Farmers.FindAsync(product.FarmerId);
                    if (farmer == null)
                    {
                        ModelState.AddModelError("", "Selected farmer does not exist.");
                        ViewBag.Farmers = _context.Farmers
                            .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                            .ToList();
                        return View(product);
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Employee"))
            {
                ViewBag.Farmers = _context.Farmers
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                    .ToList();
            }

            return View(product);
        }

        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                if (farmer == null || product.FarmerId != farmer.Id) return Forbid();
            }

            if (User.IsInRole("Employee"))
            {
                ViewBag.Farmers = _context.Farmers
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                    .ToList();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                if (farmer == null || product.FarmerId != farmer.Id) return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Employee"))
            {
                ViewBag.Farmers = _context.Farmers
                    .Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name })
                    .ToList();
            }

            return View(product);
        }

        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Farmer).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                if (farmer == null || product.FarmerId != farmer.Id) return Forbid();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (User.IsInRole("Farmer"))
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == User.Identity.Name);
                if (farmer == null || product.FarmerId != farmer.Id) return Forbid();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}