using System;
using System.Linq;
using System.Windows;

namespace Laba10_1
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly string _userEmail;

        public ChangePasswordWindow(string userEmail)
        {
            InitializeComponent();
            _userEmail = userEmail;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var newPassword = NewPasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Заполните оба поля пароля.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Находим пользователя по email
                var user = BD.GetContext().Researchers.FirstOrDefault(u => u.Email == _userEmail);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Хэшируем новый пароль и сохраняем в БД
                user.Password = PasswordHelper.HashPassword(newPassword);
                BD.GetContext().SaveChanges();

                MessageBox.Show("Пароль успешно изменён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
