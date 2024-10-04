using webapi_tutorial.Models;
using Microsoft.EntityFrameworkCore;

namespace webapi_tutorial.Datos;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {}
    public DbSet<Villa> Villas { get; set; } // We create our Table: Villas
    public DbSet<NumeroVilla> NumeroVillas { get; set; } // We create NumeroVillas Table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
                Id = 1,
                Nombre = "Villa Uno",
                Detalle = "Una villa muy bonita",
                Tarifa = 300,
                Ocupantes = 3,
                MetrosCuadrados = 100,
                Amenidad = "Aire Acondicionado",
                ImagenUrl = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
            },
            new Villa()
            {
                Id = 2,
                Nombre = "Villa Dos",
                Detalle = "Detalle de la Villa",
                Tarifa = 5500,
                Ocupantes = 100,
                MetrosCuadrados = 9876000,
                Amenidad = "",
                ImagenUrl = ""
            }
        );
    }
}



