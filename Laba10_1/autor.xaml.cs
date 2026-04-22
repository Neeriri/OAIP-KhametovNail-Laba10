using System.Linq;
using System.Windows;

namespace Laba10_1
{
    public partial class autor : Window
    {
        public autor()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTB.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите Email и пароль.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var user = BD.GetContext().Researchers.FirstOrDefault(u => u.Email == email);

                if (user != null && PasswordHelper.VerifyPassword(password, user.Password))
                {
                    MessageBox.Show("Авторизация успешна!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    var mainWindow = new MainWindow();
                    mainWindow.LoadUserData(user);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверный Email или пароль!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}\n\nПроверьте настройки подключения в файле bd.cs", "Ошибка БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecoverButton_Click(object sender, RoutedEventArgs e)
        {
            new recovery().ShowDialog();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            new Window1().ShowDialog();
        }
    }
}