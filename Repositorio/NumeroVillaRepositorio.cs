using webapi_tutorial.Datos;
using webapi_tutorial.Models;
using webapi_tutorial.Repositorio;
using webapi_tutorial.Repositorio.IRepositorio;

namespace webapi_tutorial;

public class NumeroVillaRepositorio: Repositorio<NumeroVilla>, INumeroVillaRepositorio
{
    private readonly ApplicationDbContext _db;
    public NumeroVillaRepositorio(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
    {
        entidad.FechaActualizacion = DateTime.Now;
        _db.NumeroVillas.Update(entidad);
        await _db.SaveChangesAsync();
        return entidad;
    }
}

// Add service in Program.cs