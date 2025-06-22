using System.Net;
using System.Net.Mail;

namespace Electronics.Presentation.Utility
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = "techbazarbd.com@gmail.com"; // your email
            var password = "pllw iwwp jmda cvtk";         // app password or SMTP password

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var message = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true // ✅ THIS IS IMPORTANT
            };

            await smtp.SendMailAsync(message);
        }
    }
}
