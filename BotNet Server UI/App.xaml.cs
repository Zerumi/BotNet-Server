// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Linq;
using System.Reflection;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class Application : System.Windows.Application
    {
        private void App_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            try
            {
                CheckDll();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString() + "\nОшибки возникшие во время запуска не позволяют продолжать бесперебойную работу программы.\nУстраните эти ошибки прежде чем начать использование программы");
                Environment.Exit(0);
            }
        }
        private async void CheckDll()
        {
            try
            {
                _ = Assembly.Load("m3md2");
                _ = Assembly.Load("m3md2_startup");
                _ = Assembly.Load("CommandsLibrary");
                VerifyVersion version = await ApiRequest.GetProductAsync<VerifyVersion>($"api/v1/support/versions/{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
                if (version.isDeprecated)
                {
                    throw new NotSupportedException("Данная версия программы устарела. Пожалуйста, загрузите новую версию в разделе Releases");
                }
                else if (version.isUpdateNeeded)
                {
                    System.Windows.MessageBox.Show("Данная версия программы скоро устареет. Пожалуйста, загрузите новую версию программы в разделе Releases", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                else if (version.isNotLatest)
                {
                    System.Windows.MessageBox.Show("Доступно обновление для этой программы, которое содержит множество исправлений и улучшений, вы можете загрузить его в разделе Releases", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
                var cmdlib = CommandsLibrary.Verifier.Verify();
                var m3md = m3md2.Verifier.Verify();
                var m3md_startup = m3md2_startup.Verifier.Verify();
                if (!(version.cmdlib.Contains(cmdlib.Item2) && version.m3md2.Contains(m3md.Item2) && version.m3md2_startup.Contains(m3md_startup.Item2)))
                {
                    throw new PlatformNotSupportedException("Данная версия библиотеки не поддерживается этим экземпляром оболочки");
                }
                Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString() + "\nОшибки возникшие во время запуска не позволяют продолжать бесперебойную работу программы.\nУстраните эти ошибки прежде чем начать использование программы");
                Environment.Exit(0);
            }
        }
        private void Start()
        {
            try
            {
                m3md2_startup.StartupManager.Main();
                System.Net.ServicePointManager.Expect100Continue = System.Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("Expect100Continue"));
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"(StartupManager) Значение Expect100Continue установлено на {System.Net.ServicePointManager.Expect100Continue}\n";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString() + "\nОшибки возникшие во время запуска не позволяют продолжать бесперебойную работу программы.\nУстраните эти ошибки прежде чем начать использование программы");
                Environment.Exit(0);
            }
        }
    }
}
