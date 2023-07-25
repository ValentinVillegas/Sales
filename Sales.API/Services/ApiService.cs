using Newtonsoft.Json;
using Sales.Shared.Responses;
using System.Text.Json.Serialization;

namespace Sales.API.Services
{
    public class ApiService : IApiService
    {
        private readonly string _urlBase;
        private readonly string _tokenName;
        private readonly string _tokenValue;

        public ApiService(IConfiguration configuration)
        {
            _urlBase = configuration["CountriesAPI:urlBase"]!;
            _tokenName = configuration["CountriesAPI:tokenName"]!;
            _tokenValue = configuration["CountriesAPI:tokenValue"]!;
        }

        public async Task<Response> GetListAsync<T>(string servicePrefix, string controller)
        {
            try
            {
                HttpClient client = new HttpClient { 
                    BaseAddress = new Uri(_urlBase),
                };

                client.DefaultRequestHeaders.Add(_tokenName, _tokenValue);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSucces = false,
                        Message = result
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(result)!;

                return new Response
                {
                    IsSucces = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucces = false,
                    Message = ex.Message
                };
            }
        }
    }
}
