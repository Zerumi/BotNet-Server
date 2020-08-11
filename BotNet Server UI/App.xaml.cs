// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Threading;

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
                ApiRequest.OnRequestFailed += ApiRequest_OnRequestFailed;
                Start();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString() + "\nОшибки возникшие во время запуска не позволяют продолжать бесперебойную работу программы.\nУстраните эти ошибки прежде чем начать использование программы");
                Environment.Exit(0);
            }
        }

        private void ApiRequest_OnRequestFailed(Exception ex)
        {
            m3md2.StaticVariables.Settings.IsDataProblem.Add(true);
            Thread thread = new Thread(StartVisualize);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private void StartVisualize()
        {
            NetworkErrorVisualiser visualiser = new NetworkErrorVisualiser(m3md2.StaticVariables.Settings.IsDataProblem.Count - 1)
            {
                Focusable = false,
                ShowActivated = false
            };
            visualiser.ShowDialog();
        }

        private void Start()
        {
            try
            {
                m3md2_startup.StartupManager.Main();
                System.Net.ServicePointManager.Expect100Continue = System.Convert.ToBoolean(ConfigurationRequest.GetValueByKey("Expect100Continue"));
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
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
