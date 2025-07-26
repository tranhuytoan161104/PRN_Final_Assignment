using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Final.UserAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.SmtpUser));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                _logger.LogInformation("Đang chuẩn bị gửi email đến {ToEmail}...", toEmail);

                // Kết nối tới Gmail SMTP
                await client.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);

                // Xác thực
                await client.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                // Gửi email  
                await client.SendAsync(message);

                // Ngắt kết nối
                await client.DisconnectAsync(true);

                _logger.LogInformation("Gửi email đến {ToEmail} thành công.", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gửi email đến {ToEmail} thất bại.", toEmail);
                throw;
            }
        }
    }
}