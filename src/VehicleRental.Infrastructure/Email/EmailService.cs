using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using VehicleRental.Application.Abstractions.Email;
using VehicleRental.Infrastructure.Email.Settings;

namespace VehicleRental.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly GmailSettings _settings;

        public EmailService(ILogger<EmailService> logger, IOptions<GmailSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task SendAsync(Domain.Users.Email recipient, string subject, string body)
        {

            try
            {
                var from = _settings.Username;
                var password = _settings.Password;
                var message = new MailMessage();
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(recipient.Value));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = _settings.Port,
                    Credentials = new NetworkCredential(from, password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation("Email send to: {Recipient}", recipient.Value);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending email}", ex);
                throw new Exception("Error sending email", ex);
            }

            await Task.CompletedTask;
        }
    }
}
