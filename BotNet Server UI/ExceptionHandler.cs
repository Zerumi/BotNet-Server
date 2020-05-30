// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BotNet_Server_UI
{
    static class ExceptionHandler
    {
        private static void RegisterToM3MD2(Exception ex)
        {
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
            m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
            m3md2.StaticVariables.Diagnostics.ExceptionCount++;
        }
        public static void RegisterNew(Exception ex)
        {
            if (!m3md2.StaticVariables.Settings.IsDataProblem.Contains(true))
            {
                MessageBox.Show(ex.ToString());
                RegisterToM3MD2(ex);
            }
            else
            {
                if (!(ex.GetType() == typeof(NullReferenceException)))
                {
                    RegisterToM3MD2(ex);
                }
            }
        }
        public static void RegisterNew(Exception ex, bool iswithmessage)
        {
            if (iswithmessage)
            {
                MessageBox.Show(ex.ToString());
            }
            RegisterToM3MD2(ex);
        }
    }
}
