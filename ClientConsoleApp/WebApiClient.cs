using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientConsoleApp
{
    public class WebApiClient : IDisposable
    {
        private HttpClient _client;

        private JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings
        {
            DateFormatString = "yy.MM+dd"
        };

        public WebApiClient(string baseAddress)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Brotli
            };

            _client = new HttpClient(handler) { 
                BaseAddress = new Uri(baseAddress) };
            _client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("br"));
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> GetAsync<T>(string request)
        {
            var content = await GetStringAsync(request);
            if (content == null)
                return default;
            return JsonConvert.DeserializeObject<T>(content, JsonSerializerSettings);

        }
        public async Task<string> GetStringAsync(string request)
        {
            var response = await _client.GetAsync(request);
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
                return default;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> PostAsync<T>(string request, T payload)
        {
            var response = await _client.PostAsJsonAsync(request, payload);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task LoginAsync<T>(string request, T payload)
        {
            var response = await _client.PostAsJsonAsync(request, payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", content);
        }
    }
}
