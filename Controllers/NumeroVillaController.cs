using System.Net;
using AutoMapper;
using webapi_tutorial.Models;
using Microsoft.AspNetCore.Mvc;
using webapi_tutorial.Repositorio.IRepositorio;
using webapi_tutorial.Models.Dto;

namespace webapi_tutorial.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NumeroVillaController : ControllerBase
{
    private readonly ILogger<NumeroVillaController> _logger;
    private readonly IVillaRepositorio _villaRepo;
    private readonly INumeroVillaRepositorio _numeroRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo, IMapper mapper)
    {
        _logger = logger;
        _villaRepo = villaRepo;
        _numeroRepo = numeroRepo;
        _mapper = mapper;
        _response = new();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetNumeroVillas()
    {
        try
        {
            _logger.LogInformation("GetNumeroVillas init");
            IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();
            _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
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
    [HttpGet("GetSoloNumeroVilla", Name = "GetNumeroVilla")] // http://127.0.0.1:8000/api/Villa/GetNumeroVilla?id=1
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id) // id is the Query Param Name
    {
       try
       {
            if (id == 0)
            {
                _logger.LogError("GetNumeroVilla Id: " + id);
                _response.statusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                return BadRequest(_response);
            }
            var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

            if (numeroVilla == null)
            {
                _response.statusCode = HttpStatusCode.NotFound;
                _response.IsExitoso = false;

                return NotFound(_response);
            }
            _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
            _response.statusCode = HttpStatusCode.OK;

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
    public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
    {
       try
       {
            // Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Custom Validation
            if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
            {
                ModelState.AddModelError("NOMBRE_EXISTE", "La Villa Con Este Nombre No Existe");
                return BadRequest(ModelState);
            }
            if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
            {
                ModelState.AddModelError("NO_EXISTE", "LA_VILLA_NO_EXISTE");
                return BadRequest(ModelState);
            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            
            NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
            modelo.FechaCreacion = DateTime.Now;
            modelo.FechaActualizacion = DateTime.Now;
           
            await _numeroRepo.Crear(modelo);
            _response.Resultado = modelo;
            _response.statusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetNumeroVilla", new {id = modelo.VillaNo}, _response);
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
    public async Task<IActionResult> DeleteNumeroVilla(int id)
    {
        try
        {
            if (id==0)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.BadRequest;

                return BadRequest(_response);
            }
            var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
            if (numeroVilla == null)
            {
                _response.IsExitoso = false;
                _response.statusCode = HttpStatusCode.NotFound;

                return NotFound(_response);
            }

            await _numeroRepo.Remover(numeroVilla);

            _response.statusCode = HttpStatusCode.NoContent;

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
    public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
    {
        if (updateDto == null || id != updateDto.VillaNo)
        {
            _response.IsExitoso = false;
            _response.statusCode = HttpStatusCode.BadRequest;

            return BadRequest(_response);
        }
        if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
        {
            ModelState.AddModelError("ForeignKey", "El id no existe");
            return BadRequest(ModelState);
        }

        NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

        await _numeroRepo.Actualizar(modelo);
        _response.statusCode = HttpStatusCode.NoContent;

        return Ok(_response);
    }
}
