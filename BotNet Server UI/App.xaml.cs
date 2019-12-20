// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class Application : System.Windows.Application
    {
        private void App_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            m3md2_startup.StartupManager.Main();
        }
    }
}
