using Sales.Shared.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Mobile.Services
{
    public interface ICategoriasService
    {
        Task<List<Categoria>> ObtenerCategoriasAsync();
    }
}
