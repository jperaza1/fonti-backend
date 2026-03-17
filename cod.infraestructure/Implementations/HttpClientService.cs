using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using cod.application.Interfaces;

namespace cod.infraestructure.Implementations
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _httpClient;
        public HttpClientService()
        {
            _httpClient = new HttpClient();
        }

        public void NewInstance()
        {

            _httpClient = new HttpClient();

        }

        public void AddHeader(string key, string value)
        {

            _httpClient.DefaultRequestHeaders.Add(key, value);

        }

        public void AddHeaderAutorization(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }

        public async Task<T> PostResult<T>(string urlClient, object model)
        {
            try
            {

                var objectSend = GetObject(model);

                var result = await _httpClient.PostAsync(urlClient, objectSend);

                var response = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(response);

            }
            catch (Exception exception)
            {

                return default;

            }

        }
        public async Task<T> GetResult<T>(string urlClient)
        {
            try
            {

                var result = await _httpClient.GetAsync(urlClient).ConfigureAwait(false);

                var response = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(response);

            }
            catch (Exception)
            {

                return default;

            }

        }
        public ByteArrayContent GetObject<T>(T model)
        {

            var httpContext = JsonConvert.SerializeObject(model);

            var buffer = Encoding.UTF8.GetBytes(httpContext);

            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;

        }
    }
}
