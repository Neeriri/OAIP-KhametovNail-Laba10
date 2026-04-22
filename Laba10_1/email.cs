using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Laba10_1
{
    public static class EmailService
    {
        private const string SmtpHost = "smtp.mail.ru";
        private const int SmtpPort = 587;
        private const string SenderEmail = "your_email@mail.ru";
        private const string SenderAppPassword = "your_20_char_app_password";

        public static string SendVerificationCode(string toEmail)
        {
            string code = new Random().Next(100000, 999999).ToString();

            using (var client = new SmtpClient(SmtpHost, SmtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(SenderEmail, SenderAppPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail, "Восстановление доступа"),
                    Subject = "Код подтверждения",
                    Body = $"<h1>Ваш код подтверждения: <b>{code}</b></h1>",
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };

                mailMessage.To.Add(toEmail);
                client.Send(mailMessage);

                return code;
            }
        }
    }
}