using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Models.DbContext
{
    public class AppointmentDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Services> Services { get; set; }
    }
}
