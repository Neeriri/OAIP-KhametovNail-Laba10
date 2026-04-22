using System;
using System.Linq;
using System.Windows;

namespace Laba10_1
{
    public partial class recovery : Window
    {
        private string _generatedCode;

        public recovery()
        {
            InitializeComponent();
        }

        private void SendCodeButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTB.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите Email.", "Ошибка");
                return;
            }

            var user = BD.GetContext().Researchers.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден.", "Ошибка");
                return;
            }

            try
            {
                _generatedCode = EmailService.SendVerificationCode(email);
                MessageBox.Show("Код отправлен на указанную почту.", "Успех");
                CodePanel.Visibility = Visibility.Visible;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                MessageBox.Show($"Ошибка SMTP: {ex.Message}\nПроверьте настройки EmailService.", "Ошибка");
            }
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (CodeTB.Text == _generatedCode)
            {
                MessageBox.Show("Код подтверждён! В реальном приложении здесь открывается форма смены пароля.", "Успех");
                Close();
            }
            else
            {
                MessageBox.Show("Неверный код.", "Ошибка");
            }
        }
    }
}