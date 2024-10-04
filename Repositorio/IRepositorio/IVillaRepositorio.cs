using webapi_tutorial.Models;

namespace webapi_tutorial.Repositorio.IRepositorio;

public interface IVillaRepositorio: IRepositorio<Villa>
{
    Task<Villa> Actualizar(Villa entidad);
}
