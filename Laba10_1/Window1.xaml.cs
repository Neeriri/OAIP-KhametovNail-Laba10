using System;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Laba10_1
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
         
            if (string.IsNullOrWhiteSpace(FirstNameTB.Text) ||
                string.IsNullOrWhiteSpace(LastNameTB.Text) ||
                string.IsNullOrWhiteSpace(EmailTB.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(PhoneTB.Text) ||
                string.IsNullOrWhiteSpace(ResearchFieldTB.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(DOBPicker.Text, out DateTime dob))
            {
                MessageBox.Show("Неверный формат даты рождения.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(PubDatePicker.Text, out DateTime pubDate))
            {
                MessageBox.Show("Неверный формат даты публикации.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(EmailTB.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный формат Email.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordBox.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var researcher = new Researcher
            {
                FirstName = FirstNameTB.Text,
                LastName = LastNameTB.Text,
                DateOfBirth = dob,
                Email = EmailTB.Text,
                Password = PasswordHelper.HashPassword(PasswordBox.Password),
                PhoneNumber = PhoneTB.Text,
                ResearchField = ResearchFieldTB.Text,
                FirstPublicationDate = pubDate,
                Role = AdminCheckBox.IsChecked == true ? "Administrator" : "User"
            };

            try
            {
                using var context = new BD();

                if (context.Researchers.Any(r => r.Email == researcher.Email))
                {
                    MessageBox.Show("Пользователь с таким Email уже существует!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                context.Researchers.Add(researcher);

                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
                context.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                new autor().Show();
                Close();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}\n\nПроверьте настройки подключения в файле bd.cs", "Ошибка БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
       
            catch (Exception ex)
            {
                string fullError = $"Основная ошибка: {ex.Message}";

                // Распаковываем вложенные исключения
                var inner = ex.InnerException;
                int level = 1;
                while (inner != null)
                {
                    fullError += $"\n[Уровень {level}] {inner.GetType().Name}: {inner.Message}";
                    inner = inner.InnerException;
                    level++;
                }

                // Если это ошибка PostgreSQL, выводим её специфичные поля
                if (ex is Npgsql.PostgresException pgEx)
                {
                    fullError += $"\n\n🔴 PostgreSQL Error:\n" +
                                 $"SQL State: {pgEx.SqlState}\n" +
                                 $"Message Text: {pgEx.MessageText}\n" +
                                 $"Detail: {pgEx.Detail}\n" +
                                 $"Hint: {pgEx.Hint}";
                }

                MessageBox.Show(fullError, "Детали ошибки БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}