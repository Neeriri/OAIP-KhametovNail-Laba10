using System;
using System.Text.RegularExpressions;
using System.Windows;

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
                FirstPublicationDate = pubDate
            };

            try
            {
                BD.GetContext().Researchers.Add(researcher);
                BD.GetContext().SaveChanges();

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
                MessageBox.Show($"Ошибка БД: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}