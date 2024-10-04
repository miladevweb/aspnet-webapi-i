using System.Net;
using AutoMapper;
using webapi_tutorial.Models;
using Microsoft.AspNetCore.Mvc;
using webapi_tutorial.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using webapi_tutorial.Repositorio.IRepositorio;

namespace webapi_tutorial.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController: ControllerBase
{
    private readonly ILogger<VillaController> _logger;
    // private readonly ApplicationDbContext _db;
    private readonly IVillaRepositorio _villaRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;
    public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo,IMapper mapper) //  ApplicationDbContext db
    {
        _logger = logger;
        _mapper = mapper;
        _villaRepo = villaRepo;
        _response = new();
        // _db = db;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        _logger.LogInformation("Obteniendo todas las villas");
        // return Ok(VillaStore.villaList);
        // return Ok(await _db.Villas.ToListAsync());

        // IEnumerable<Villa> villaList  = await _db.Villas.ToListAsync();

        try
        {
            IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();

            _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.statusCode = HttpStatusCode.OK;

            // return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string> { ex.Message };            
        }
        return _response;
        
    }

    // Without keys is a subroute /a/b/c/...
    [HttpGet("GetSoloVilla", Name = "GetVilla")] // http://127.0.0.1:8000/api/Villa/GetSoloVilla?id=1
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<ActionResult<VillaDto>> GetVilla(int id) // id is the Query Param Name
    public async Task<ActionResult<APIResponse>> GetVilla(int id) // id is the Query Param Name
    {
       try
       {
            if (id == 0)
            {
                _logger.LogError("Error al traer Villa con Id: " + id);
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                // return BadRequest();
                return BadRequest(_response);
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            // var villa = await  _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if (villa == null)
            {
                _response.statusCode = HttpStatusCode.NotFound;
                _response.IsExitoso = false;

                // return NotFound();
                return NotFound(_response);
            }
            _response.Resultado = _mapper.Map<VillaDto>(villa);
            _response.statusCode = HttpStatusCode.OK;

            // return Ok(villa);
            //  return Ok(_mapper.Map<VillaDto>(villa));
            return Ok(_response);
       }
       catch (Exception ex)
       {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string> { ex.Message };
       }
       return _response;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult<VillaDto>> CrearVilla([FromBody] VillaCreateDto createDto)
    public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto createDto)
    {
       try
       {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Custom Validation
            // if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            // if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NOMBRE_EXISTE", "La Villa Con Este Nombre No Existe");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            
            // createDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;

            // var lastVilla = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault();
            // createDto.Id = (lastVilla != null ? lastVilla.Id : 0) + 1; // Ternary
            // VillaStore.villaList.Add(createDto);

            // Villa modelo = new()
            // {
            //     Nombre = createDto.Nombre,
            //     Amenidad = createDto.Amenidad,
            //     Ocupantes = createDto.Ocupantes,
            //     MetrosCuadrados = createDto.MetrosCuadrados,
            //     ImagenUrl = createDto.ImagenUrl,
            //     Tarifa = createDto.Tarifa,
            //     Detalle = createDto.Detalle
            // };
            Villa modelo = _mapper.Map<Villa>(createDto);
            modelo.FechaCreacion = DateTime.Now;
            modelo.FechaActualizacion = DateTime.Now;
            // await _db.Villas.AddAsync(modelo);
            // await _db.SaveChangesAsync();

            await _villaRepo.Crear(modelo);
            _response.Resultado = modelo;
            _response.statusCode = HttpStatusCode.Created;

            // return CreatedAtRoute("GetVilla", new {id = modelo.Id}, modelo);
            return CreatedAtRoute("GetVilla", new {id = modelo.Id}, _response);
       }
       catch (Exception ex)
       {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string> { ex.Message };
       }
       return _response;
    }


    // With keys is a param: {ParamName:type}
    [HttpDelete("{id:int}")] // http://127.0.0.1:8000/api/Villa/1
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        try
        {
            if (id==0)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;

                // return BadRequest();
                return BadRequest(_response);
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            // var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            var villa = await _villaRepo.Obtener(v => v.Id == id);
            if (villa == null)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.NotFound;

                // return NotFound();
                return NotFound(_response);
            }
            // VillaStore.villaList.Remove(villa);

            // _db.Villas.Remove(villa);
            // await _db.SaveChangesAsync();

            await _villaRepo.Remover(villa);

            _response.statusCode = HttpStatusCode.NoContent;

            // return NoContent();
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        // Las interfaces no pueden tener un tipo como genérico APIResponse, por ende tenemos que retornar la _response de esta manera para que no de error.
        return BadRequest(_response);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
    {
        if (updateDto == null || id != updateDto.Id)
        {
            _response.IsExitoso = false;
            _response.statusCode = HttpStatusCode.BadRequest;

            // return BadRequest();
            return BadRequest(_response);
        }
        // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
        // if (villa == null)
        // {
        //     return BadRequest();
        // }
        // villa.Nombre = updateDto.Nombre;
        // villa.Ocupantes = updateDto.Ocupantes;
        // villa.MetrosCuadrados = updateDto.MetrosCuadrados;

        // Villa villa = new()
        // {
        //     Id = updateDto.Id,
        //     Nombre = updateDto.Nombre,
        //     Ocupantes = updateDto.Ocupantes,
        //     MetrosCuadrados = updateDto.MetrosCuadrados,
        //     ImagenUrl = updateDto.ImagenUrl,
        //     Tarifa = updateDto.Tarifa,
        //     Detalle = updateDto.Detalle,
        //     Amenidad = updateDto.Amenidad,
        // };
        Villa villa = _mapper.Map<Villa>(updateDto);
        // _db.Villas.Update(villa);
        // await _db.SaveChangesAsync();

        await _villaRepo.Actualizar(villa);
        _response.statusCode = HttpStatusCode.NoContent;

        // return NoContent();
        return Ok(_response);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
    {
        try
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            // var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            // var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);
            if (villa == null)
            {
                return BadRequest();
            }
            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            // VillaUpdateDto villaDto = new()
            // {
            //     Id = villa.Id,
            //     Nombre = villa.Nombre,
            //     Ocupantes = villa.Ocupantes,
            //     MetrosCuadrados = villa.MetrosCuadrados,
            //     ImagenUrl = villa.ImagenUrl,
            //     Tarifa = villa.Tarifa,
            //     Detalle = villa.Detalle,
            //     Amenidad = villa.Amenidad
            // };
            // patchDto.ApplyTo(villa, ModelState);
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Villa modelo = new()
            // {
            //     Id = villaDto.Id,
            //     Nombre = villaDto.Nombre,
            //     Ocupantes = villaDto.Ocupantes,
            //     MetrosCuadrados = villaDto.MetrosCuadrados,
            //     ImagenUrl = villaDto.ImagenUrl,
            //     Tarifa = villaDto.Tarifa,
            //     Detalle = villaDto.Detalle,
            //     Amenidad = villaDto.Amenidad
            // };

            // _db.Villas.Update(modelo);
            // await _db.SaveChangesAsync();

            Villa modelo = _mapper.Map<Villa>(villaDto);
            await _villaRepo.Actualizar(modelo);

            // return NoContent();
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string> { ex.Message };            
        }
        return Ok(_response);
    }
}