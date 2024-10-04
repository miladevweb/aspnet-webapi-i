﻿using System.ComponentModel.DataAnnotations;

namespace webapi_tutorial.Models.Dto;

public class VillaUpdateDto
{
    [Required]
    public int Id { get; set; }

    // Validations
    [Required] 
    [MaxLength(30)]
    public required string Nombre { get; set; }
    public string Detalle { get; set; }

    [Required]
    public double Tarifa { get; set; }
    [Required]
    public string ImagenUrl { get; set; }
    public string Amenidad { get; set; }
    [Required]
    public int Ocupantes { get; set; }
    [Required]
    public int MetrosCuadrados { get; set; }
}