using System.ComponentModel.DataAnnotations;

namespace webapi_tutorial.Models.Dto;

public class VillaDto
{
    public int Id { get; set; }

    // Validations
    [Required] 
    [MaxLength(30)]
    public required string Nombre { get; set; }
    public string Detalle { get; set; }

    [Required]
    public double Tarifa { get; set; }
    public string ImagenUrl { get; set; }
    public string Amenidad { get; set; }
    public int Ocupantes { get; set; }
    public int MetrosCuadrados { get; set; }
}
