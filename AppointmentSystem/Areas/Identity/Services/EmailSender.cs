using Microsoft.AspNetCore.Identity.UI.Services;

namespace AppointmentSystem.Areas.Identity.Services
{
    public class DefaultEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
