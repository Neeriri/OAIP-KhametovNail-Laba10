using System.Windows;

namespace Laba10_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void LoadUserData(Researcher user)
        {
            UserInfoTextBlock.Text = $"👤 {user.LastName} {user.FirstName}\n" +
                                   $"📅 Дата рождения: {user.DateOfBirth:dd.MM.yyyy}\n" +
                                   $"📧 Email: {user.Email}\n" +
                                   $"📱 Телефон: {user.PhoneNumber}\n" +
                                   $"🔬 Область исследования: {user.ResearchField}\n" +
                                   $"📚 Дата первой публикации: {user.FirstPublicationDate:dd.MM.yyyy}";
        }
    }
}