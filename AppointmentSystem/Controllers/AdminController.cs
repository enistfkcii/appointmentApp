using AppointmentSystem.Models;
using AppointmentSystem.Models.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppointmentDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, AppointmentDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Appointments()
        {
            var appointments = _context.Appointments.Include(a => a.Service)
                                       .OrderBy(a => a.AppointmentDate)
                                       .ToList();
            return View(appointments);
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Json(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            
            var currentRoles = await _userManager.GetRolesAsync(user);

            
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                return BadRequest(removeRolesResult.Errors);
            }

            
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, role);
            if (addRoleResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest(addRoleResult.Errors);
        }

        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok(user);
            }

            return BadRequest(result.Errors);
        }

    }
}
