using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Laba10_1
{
    public static class EmailService
    {
        // Настройки SMTP для отправки писем
        // ДЛЯ MAIL.RU:
        //   SmtpHost = "smtp.mail.ru", SmtpPort = 587
        //   Нужно создать "пароль приложения" в настройках почты
        // ДЛЯ GMAIL:
        //   SmtpHost = "smtp.gmail.com", SmtpPort = 587
        //   Нужно включить двухфакторную аутентификацию и создать "пароль приложения"
        // ДЛЯ YANDEX:
        //   SmtpHost = "smtp.yandex.ru", SmtpPort = 465
        
        // !!! ЗАМЕНИТЕ НА ВАШИ РЕАЛЬНЫЕ ДАННЫЕ !!!
        private const string SmtpHost = "smtp.mail.ru";
        private const int SmtpPort = 587;
        private const string SenderEmail = "REPLACE_WITH_YOUR_EMAIL@mail.ru";
        private const string SenderAppPassword = "REPLACE_WITH_YOUR_APP_PASSWORD";

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
                    Body = $"<h1>Ваш код подтверждения: <b>{code}</b></h1><p>Код действителен в течение 15 минут.</p>",
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