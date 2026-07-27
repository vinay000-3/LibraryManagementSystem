using LibraryManagementSystem.EmailConfiguration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LibraryManagementSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string fullName)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _settings.SenderName,
                _settings.SenderEmail));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = "Welcome to Library Management System";

            email.Body = new TextPart("html")
            {
                Text = $@"
                    <h2>Welcome {fullName}!</h2>

                    <p>Your account has been created successfully.</p>

                    <p>You can now login and start borrowing books.</p>

                    <br/>

                    <b>Library Management System</b>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _settings.SmtpServer,
                _settings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _settings.SenderEmail,
                _settings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}