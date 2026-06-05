using GestorDeTareas.Application.DTOs;
using GestorDeTareas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GestorDeTareas.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _tareaService;

    public TareasController(ITareaService tareaService)
    {
        _tareaService = tareaService;
    }

    private Guid ObtenerUsuarioId()
    {
        string? usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (usuarioId == null)
        {
            throw new UnauthorizedAccessException("No se ha encontrado el usuario autenticado.");
        }

        return Guid.Parse(usuarioId);
    }

    [HttpGet]
    public ActionResult<List<TareaResponseDto>> ObtenerTodas()
    {
        Guid usuarioId = ObtenerUsuarioId();
        List<TareaResponseDto> tareas = _tareaService.ObtenerTodas(usuarioId);

        return Ok(tareas);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<TareaResponseDto> ObtenerPorId(Guid id)
    {
        Guid usuarioId = ObtenerUsuarioId();
        TareaResponseDto? tarea = _tareaService.ObtenerPorId(id, usuarioId);

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
            Guid usuarioId = ObtenerUsuarioId();
            TareaResponseDto tareaCreada = _tareaService.Crear(crearTareaDto, usuarioId);

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
            Guid usuarioId = ObtenerUsuarioId();
            bool actualizada = _tareaService.Actualizar(id, actualizarTareaDto, usuarioId);

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
        Guid usuarioId = ObtenerUsuarioId();
        bool eliminada = _tareaService.Eliminar(id, usuarioId   );

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
            Guid usuarioId = ObtenerUsuarioId();
            bool completada = _tareaService.Completar(id, usuarioId);

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