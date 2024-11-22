using System.Security.Claims;
using AppointmentSystem.Models;
using AppointmentSystem.Models.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.Controllers
{

    public class AppointmentController : Controller
    {
        private readonly AppointmentDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public AppointmentController(AppointmentDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsAsync()
        {
            IEnumerable<Appointment> appointments;
            var user = await _userManager.GetUserAsync(User);

            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                appointments = _context.Appointments.Include(a => a.Service)
                                       .OrderBy(a => a.AppointmentDate)
                                       .ToList();
                return PartialView("Appointment/Partial/AppointmentList", appointments);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                appointments = _context.Appointments.Include(a => a.Service)
                                           .Where(a => a.UserId == userId)
                                           .OrderBy(a => a.AppointmentDate)
                                           .ToList();
            }

            return PartialView("Appointment/Partial/AppointmentList", appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Appointment appointment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                appointment.UserId = _userManager.GetUserId(User);
            }
            appointment.Status = "Beklemede";
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            var createdAppointment = await _context.Appointments
            .Include(a => a.Service) 
            .FirstOrDefaultAsync(a => a.Id == appointment.Id);
            return Ok(createdAppointment);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(Appointment model)
        {
            var user = await _userManager.GetUserAsync(User);
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == model.Id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Name = model.Name;
            appointment.Surname = model.Surname;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Description = model.Description;
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                appointment.Status = model.Status;
            }
            appointment.ServiceId = model.ServiceId;
            _context.SaveChanges();

            return Ok(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound(new { success = false, message = "Randevu bulunamadı." });
            }

            return Json(new
            {
                id = appointment.Id,
                name = appointment.Name,
                surname = appointment.Surname,
                appointmentDate = appointment.AppointmentDate,
                description = appointment.Description,
                status = appointment.Status,
                serviceId = appointment.ServiceId
            });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _context.Services.Select(s => new
            {
                s.Id,
                s.Name
            }).ToList();

            return Json(services);
        }
    }


}
