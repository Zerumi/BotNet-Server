using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;

namespace BotNet_Server_UI
{
    static class ApiRequest
    {
        public static async Task<Uri> CreateProductAsync<T>(T product, string apilist)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                client.BaseAddress = new Uri("http://botnet-api.glitch.me/");
                string json = new JavaScriptSerializer().Serialize(product);
                response = await client.PostAsync($"api/v1/{apilist}", new StringContent(json));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            // return URI of the created resource.
            return response.Headers.Location;
        }

        public static async Task<T> GetProductAsync<T>(string path)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://botnet-api.glitch.me/");
            T product = default(T);
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<T>();
            }
            return product;
        }

        public static async Task<T> UpdateProductAsync<T>(T product, string apilist, uint id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://botnet-api.glitch.me/");
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/v1/{apilist}/{id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<T>();
            return product;
        }

        public static async Task<HttpStatusCode> DeleteProductAsync(uint id, string apilist)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://botnet-api.glitch.me/");
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/v1/{apilist}/{id}");
            return response.StatusCode;
        }
    }
}
