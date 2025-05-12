using System.Threading.Tasks;
using FarmerConnectApplication.Data;
using FarmerConnectApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmerConnectApplication.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public FarmersController(ApplicationDbContext context,
                                 UserManager<IdentityUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Farmer farmer)
        {
            if (ModelState.IsValid)
            {

                var identityUser = new IdentityUser { UserName = farmer.Email, Email = farmer.Email };
                var result = await _userManager.CreateAsync(identityUser, farmer.Password);

                if (result.Succeeded)
                {

                    if (!await _roleManager.RoleExistsAsync("Farmer"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                    }

                    await _userManager.AddToRoleAsync(identityUser, "Farmer");

                    farmer.UserId = identityUser.Id;
                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(farmer);
        }
    }
}
