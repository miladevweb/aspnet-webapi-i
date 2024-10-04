using webapi_tutorial.Models.Dto;

namespace webapi_tutorial.Datos;

public static class VillaStore
{
    public static List<VillaDto> villaList = new List<VillaDto>
    {
        new VillaDto{ Id=1, Nombre="Vista a la Piscina", Ocupantes=3, MetrosCuadrados=50},
        new VillaDto{ Id=2, Nombre="Vista a la Playa", Ocupantes=100, MetrosCuadrados=200},
    };
}
