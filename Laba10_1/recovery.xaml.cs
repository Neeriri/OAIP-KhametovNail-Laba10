using System;
using System.Linq;
using System.Windows;

namespace Laba10_1
{
    public partial class recovery : Window
    {
        private string _generatedCode;
        private string _userEmail;

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

            _userEmail = email;

            try
            {
                _generatedCode = EmailService.SendVerificationCode(email);
                
                // Сохраняем код в базу данных
                var resetCode = new PasswordResetCode
                {
                    Email = email,
                    Code = _generatedCode,
                    CreatedAt = DateTime.Now,
                    IsUsed = false
                };
                
                BD.GetContext().PasswordResetCodes.Add(resetCode);
                BD.GetContext().SaveChanges();
                
                MessageBox.Show("Код отправлен на указанную почту.", "Успех");
                CodePanel.Visibility = Visibility.Visible;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                MessageBox.Show($"Ошибка SMTP: {ex.Message}\nПроверьте настройки EmailService.", "Ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
            }
        }

        private void VerifyCodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CodeTB.Text))
            {
                MessageBox.Show("Введите код.", "Ошибка");
                return;
            }

            // Проверяем код в базе данных
            var resetCode = BD.GetContext().PasswordResetCodes
                .FirstOrDefault(rc => rc.Email == _userEmail && rc.Code == CodeTB.Text && !rc.IsUsed);

            if (resetCode == null)
            {
                MessageBox.Show("Неверный код или срок действия истек.", "Ошибка");
                return;
            }

            // Проверяем, не истек ли срок действия (15 минут)
            if (DateTime.Now - resetCode.CreatedAt > TimeSpan.FromMinutes(15))
            {
                MessageBox.Show("Срок действия кода истек. Запросите новый код.", "Ошибка");
                return;
            }

            // Код верный, открываем окно смены пароля
            MessageBox.Show("Код подтверждён!", "Успех");
            
            var changePasswordWindow = new ChangePasswordWindow(_userEmail);
            changePasswordWindow.ShowDialog();
            
            // Помечаем код как использованный
            resetCode.IsUsed = true;
            BD.GetContext().SaveChanges();
            
            Close();
        }
    }
}