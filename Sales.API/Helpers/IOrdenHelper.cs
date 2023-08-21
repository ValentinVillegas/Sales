using Sales.Shared.Responses;

namespace Sales.API.Helpers
{
    public interface IOrdenHelper
    {
        Task<Response> ProcesarOrdenAsync(string email, string comentarios);
    }
}
