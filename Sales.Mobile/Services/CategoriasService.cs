using Sales.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Mobile.Services
{
    public class CategoriasService : ICategoriasService
    {
        private readonly IRequestProvider _requestProvider;

        public CategoriasService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            var responseHttp = await _requestProvider.GetAsync<List<Categoria>>("/api/categorias");

            return responseHttp.Response;
        }
    }
}