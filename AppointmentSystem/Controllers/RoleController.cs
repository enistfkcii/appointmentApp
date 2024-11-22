using AppointmentSystem.Models;
using AppointmentSystem.Models.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentSystem.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    return Ok($"Rol '{roleName}' başarıyla oluşturuldu.");
                }
                return BadRequest("Rol oluşturulamadı.");
            }
            return BadRequest("Bu rol zaten mevcut.");
        }

        public async Task<IActionResult> AssignRoleToUser(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Kullanıcıya '{roleName}' rolü başarıyla atandı.");
            }
            return BadRequest("Rol atanamadı.");
        }
    }
}
