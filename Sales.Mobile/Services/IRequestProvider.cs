using Sales.Mobile.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Mobile.Services
{
    public interface IRequestProvider
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);
        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model);
    }
}
