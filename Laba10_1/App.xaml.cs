using System.Windows;

namespace Laba10_1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new autor().Show();
        }
    }
}