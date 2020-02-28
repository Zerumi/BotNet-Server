// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

/// <summary>
/// Предоставляет систему обращений к http://mineweb-hackserver.glitch.me/
/// </summary>
namespace BotNet_Server_UI
{
    class UpdateCenterRequest
    {
        /// <summary>
        /// Отправляет POST запрос на сервер
        /// </summary>
        /// <typeparam name="T">Класс, объект которого отправляется</typeparam>
        /// <param name="product">Экземпляр клаасса T для отправки</param>
        /// <param name="apilist">Путь в формате scripts/{apilist}</param>
        /// <returns>Асинхронную задачу с Uri этой операции</returns>
        public static async Task<Uri> CreateProductAsync<T>(T product, string apilist)
        {
            HttpResponseMessage response;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri("http://mineweb-hackserver.glitch.me/")
                };
                string json = new JavaScriptSerializer().Serialize(product);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Экземпляр класса преобразован в JSON строку {json}\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю POST запрос на {apilist}\r\n";
                response = await client.PostAsync($"scripts/{apilist}", new StringContent(json));
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Ответ от API {(response.IsSuccessStatusCode ? "Обработано успешно" : $"Что-то пошло не так {await response.Content.ReadAsStringAsync()}")}\r\n";
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
                return null;
            }
            // return URI of the created resource.
            return response.Headers.Location;
        }

        /// <summary>
        /// Отправляет GET запрос на API
        /// </summary>
        /// <typeparam name="T">Класс, объект которого отправляется</typeparam>
        /// <param name="apilist">Путь API в формате scripts/{apilist}/</param>
        /// <returns>Асинхронную задачу с экземпляром класса T</returns>
        public static async Task<T> GetProductAsync<T>(string apilist)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://mineweb-hackserver.glitch.me/")
            };
            T product = default;
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю GET запрос на {apilist}\r\n";
            HttpResponseMessage response = await client.GetAsync($"scripts/{apilist}");
            if (response.IsSuccessStatusCode)
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Запрос завершился успешно, присваиваю значение для возврата\r\n";
                product = await response.Content.ReadAsAsync<T>();
            }
            return product;
        }

        /// <summary>
        /// Отправляет DELETE запрос на API по пути apilist
        /// </summary>
        /// <param name="apilist">Путь API в формате scripts/{apilist}/</param>
        /// <returns>Асинхронную задачу со статусом операции</returns>
        public static async Task<HttpStatusCode> DeleteProductAsync(string apilist)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://mineweb-hackserver.glitch.me/")
            };
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю DELETE запрос на {apilist}\r\n";
            HttpResponseMessage response = await client.DeleteAsync(
                $"scripts/{apilist}/");
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Ответ от API {(response.IsSuccessStatusCode ? "Обработано успешно" : $"Что-то пошло не так {await response.Content.ReadAsStringAsync()}")}\r\n";
            return response.StatusCode;
        }
    }
}
