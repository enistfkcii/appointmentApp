using AppointmentSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AppointmentSystem.Areas.Identity.Services
{
    public class RoleAssignment
    {
        public static async Task AssignRole(IServiceProvider serviceProvider, string email, string roleName)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                Console.WriteLine($"Rol '{roleName}' başarıyla oluşturuldu.");
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                Console.WriteLine($"Kullanıcı '{email}' bulunamadı.");
                return;
            }

            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Kullanıcıya '{roleName}' rolü başarıyla atandı.");
                }
                else
                {
                    Console.WriteLine("Rol atama işlemi sırasında hatalar oluştu:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Kullanıcı zaten '{roleName}' rolünde.");
            }
        }
    }
}
