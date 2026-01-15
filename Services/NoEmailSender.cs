using Microsoft.AspNetCore.Identity.UI.Services;

namespace Garage_2._0.Services
{
    public class NoEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No-op: used for development / school project
            return Task.CompletedTask;
        }
    }
}
