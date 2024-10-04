using webapi_tutorial.Datos;
using webapi_tutorial.Models;
using webapi_tutorial.Repositorio.IRepositorio;

namespace webapi_tutorial.Repositorio;

public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
{
    private readonly ApplicationDbContext _db;
    public VillaRepositorio(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<Villa> Actualizar(Villa entidad)
    {
        entidad.FechaActualizacion = DateTime.Now;
        _db.Villas.Update(entidad);
        await _db.SaveChangesAsync();
        return entidad;
    }
}
