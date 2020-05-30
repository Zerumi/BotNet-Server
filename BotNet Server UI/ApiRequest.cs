// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Предоставляет систему обращения к основному API BotNet
    /// </summary>
    static class ApiRequest
    {
        /// <summary>
        /// Адрес API в сети Интернет
        /// </summary>
        public static string BaseAddress { get; set; }
        /// <summary>
        /// Версия API
        /// </summary>
        public static uint ApiVersion { get; set; }
        /// <summary>
        /// Отправляет POST запрос на API
        /// </summary>
        /// <typeparam name="T">Класс, объект которого отправляется</typeparam>
        /// <param name="product">Экземпляр клаасса T для отправки</param>
        /// <param name="apilist">Путь в формате api/v{apiversion}/{apilist}</param>
        /// <returns>Асинхронную задачу с Uri этой операции</returns>
        public static async Task<Uri> CreateProductAsync<T>(T product, string apilist)
        {
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(BaseAddress)
                };
                string json = new JavaScriptSerializer().Serialize(product);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Экземпляр класса преобразован в JSON строку {json}\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю POST запрос на {apilist}\r\n";
                response = await client.PostAsync($"api/v{ApiVersion}/{apilist}", new StringContent(json));
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Ответ от API {(response.IsSuccessStatusCode ? "Обработано успешно" : $"Что-то пошло не так {await response.Content.ReadAsStringAsync()}")}\r\n";
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                OnRequestFailed?.Invoke(ex);
            }
            // return URI of the created resource.
            return response?.Headers?.Location;
        }

        /// <summary>
        /// Отправляет GET запрос на API
        /// </summary>
        /// <typeparam name="T">Класс, объект которого отправляется</typeparam>
        /// <param name="path"></param>
        /// <returns>Асинхронную задачу с экземпляром класса T</returns>
        public static async Task<T> GetProductAsync<T>(string path)
        {
            T product = default;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(BaseAddress)
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю GET запрос на {path}\r\n";
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Запрос завершился успешно, присваиваю значение для возврата\r\n";
                    product = await response.Content.ReadAsAsync<T>();
                }
                return product;
            }
            catch (Exception ex)
            {
                OnRequestFailed?.Invoke(ex);
            }
            return product;
        }

        /// <summary>
        /// Отправляет DELETE запрос на API по пути apilist с указанием id (типизация)
        /// </summary>
        /// <param name="id">Путь API в формате api/v{apiversion}/.../{id}</param>
        /// <param name="apilist">Путь API в формате api/v{apiversion}/{apilist}/...</param>
        /// <returns>Асинхронную задачу со статусом операции</returns>
        public static async Task<HttpStatusCode> DeleteProductAsync(uint id, string apilist)
        {
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(BaseAddress)
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю DELETE запрос на {apilist}/{id}\r\n";
                response = await client.DeleteAsync(
                    $"api/v{ApiVersion}/{apilist}/{id}");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Ответ от API {(response.IsSuccessStatusCode ? "Обработано успешно" : $"Что-то пошло не так {await response.Content.ReadAsStringAsync()}")}\r\n";
                return response.StatusCode;
            }
            catch (Exception ex)
            {
                OnRequestFailed?.Invoke(ex);
            }
            return response?.StatusCode?? default;
        }

        /// <summary>
        /// Отправляет DELETE запрос на API по пути path без типизации
        /// </summary>
        /// <param name="path">Путь API</param>
        /// <returns>Асинхронную задачу со статусом операции</returns>
        public static async Task<HttpStatusCode> DeleteProductsAsync(string path)
        {
            HttpResponseMessage response = null;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(BaseAddress)
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Отправляю DELETE запрос на {path}\r\n";
                response = await client.DeleteAsync(path);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(ApiRequest) Ответ от API {(response.IsSuccessStatusCode ? "Обработано успешно" : $"Что-то пошло не так {await response.Content.ReadAsStringAsync()}")}\r\n";
}
            catch (Exception ex)
            {
                OnRequestFailed?.Invoke(ex);
            }
            return response?.StatusCode?? default;
        }

        public async static Task<T> KeepAliveGetProduct<T>(string path)
        {
            T product = default;
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(BaseAddress)
                };
                client.DefaultRequestHeaders.ConnectionClose = false;
                client.Timeout = new TimeSpan(1, 0, 0, 0);
                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    product = await response.Content.ReadAsAsync<T>();
                }
                return product;
            }
            catch (Exception ex)
            {
                OnRequestFailed?.Invoke(ex);
            }
            return product;
        }

        public delegate void ApiExHandler(Exception ex);

        public static event ApiExHandler OnRequestFailed;
    }
}
