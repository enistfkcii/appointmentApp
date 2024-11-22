using AppointmentSystem.Models;
using AppointmentSystem.Models.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace AppointmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppointmentDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AppointmentDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToAction("Index", "Admin"); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = _context.Appointments.Include(a => a.Service)
                                       .Where(a => a.UserId == userId)
                                       .OrderBy(a => a.AppointmentDate)
                                       .ToList();
            return View(appointments);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}