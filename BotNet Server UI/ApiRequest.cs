using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BotNet_Server_UI
{
    static class ApiRequest
    {
        static HttpClient client = new HttpClient();
        public static void InitializeUri()
        {
            client.BaseAddress = new Uri("http://botnet-api.glitch.me/");
        }
        public static async Task<Uri> CreateProductAsync(Message product, string apilist)
        {
            InitializeUri();
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"api/v1/{apilist}", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        public static async Task<string> GetProductAsync<T>(string path)
        {
            InitializeUri();
            T product = default(T);
            string s = "";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                s = response.Content.ReadAsStringAsync().Result;
                // product = await response.Content.ReadAsAsync<T>();
            }
            return s;
            // return product;
        }

        public static async Task<Message> UpdateProductAsync(Message product, string apilist)
        {
            InitializeUri();
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/v1/{apilist}/{product.id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            product = await response.Content.ReadAsAsync<Message>();
            return product;
        }

        public static async Task<HttpStatusCode> DeleteProductAsync(string id, string apilist)
        {
            InitializeUri();
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/v1/{apilist}/{id}");
            return response.StatusCode;
        }
    }
}
