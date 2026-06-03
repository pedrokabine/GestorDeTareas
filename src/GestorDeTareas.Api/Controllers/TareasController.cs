using GestorDeTareas.Application.DTOs;
using GestorDeTareas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeTareas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _tareaService;

    public TareasController(ITareaService tareaService)
    {
        _tareaService = tareaService;
    }

    [HttpGet]
    public ActionResult<List<TareaResponseDto>> ObtenerTodas()
    {
        List<TareaResponseDto> tareas = _tareaService.ObtenerTodas();

        return Ok(tareas);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<TareaResponseDto> ObtenerPorId(Guid id)
    {
        TareaResponseDto? tarea = _tareaService.ObtenerPorId(id);

        if (tarea == null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }

    [HttpPost]
    public ActionResult<TareaResponseDto> Crear(CrearTareaDto crearTareaDto)
    {
        try
        {
            TareaResponseDto tareaCreada = _tareaService.Crear(crearTareaDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = tareaCreada.Id },
                tareaCreada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Actualizar(Guid id, ActualizarTareaDto actualizarTareaDto)
    {
        try
        {
            bool actualizada = _tareaService.Actualizar(id, actualizarTareaDto);

            if (!actualizada)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Eliminar(Guid id)
    {
        bool eliminada = _tareaService.Eliminar(id);

        if (!eliminada)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/completar")]
    public IActionResult Completar(Guid id)
    {
        try
        {
            bool completada = _tareaService.Completar(id);

            if (!completada)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}