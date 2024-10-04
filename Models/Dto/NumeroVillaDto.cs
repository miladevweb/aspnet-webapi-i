using System.ComponentModel.DataAnnotations;

namespace webapi_tutorial.Models.Dto;

public class NumeroVillaDto
{
    [Required]
    public int VillaNo { get; set; }
    [Required]
    public int VillaId { get; set; }
    public string DetalleEspecial { get; set; }
}

// Now we must map the NumeroVillaDto to NumeroVilla model.