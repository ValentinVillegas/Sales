using Sales.Mobile.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sales.Mobile.Services
{
    internal class RequestProvider : IRequestProvider
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonDetaulOptions => new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public RequestProvider(HttpClient httpCliente)
        {
            _httpClient = httpCliente;
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = new HttpResponseMessage();

            try
            {
                responseHttp = await _httpClient.GetAsync(url);

                if (responseHttp.IsSuccessStatusCode)
                {
                    var response = await UnserializeAnswer<T>(responseHttp, _jsonDetaulOptions);
                    return new HttpResponseWrapper<T>(response, false, responseHttp);
                }

                return new HttpResponseWrapper<T>(default, true, responseHttp);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpResponseWrapper<T>(default, true, responseHttp);
            }
            
        }

        public Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            throw new NotImplementedException();
        }

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var respuestaString = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSerializerOptions);
        }
    }
}
