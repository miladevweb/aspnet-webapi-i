using webapi_tutorial.Models;

namespace webapi_tutorial.Repositorio.IRepositorio;

public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
{
    Task<NumeroVilla> Actualizar(NumeroVilla entidad);
}
