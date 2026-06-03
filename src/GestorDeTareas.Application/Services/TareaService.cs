using GestorDeTareas.Application.DTOs;
using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;
using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Application.Services;

public class TareaService : ITareaService
{
    private readonly ITareaRepository _tareaRepository;

    public TareaService(ITareaRepository tareaRepository)
    {
        _tareaRepository = tareaRepository;
    }

    public List<TareaResponseDto> ObtenerTodas()
    {
        return _tareaRepository.ObtenerTodas()
            .Select(ConvertirADto)
            .ToList();
    }

    public TareaResponseDto? ObtenerPorId(Guid id)
    {
        Tarea? tarea = _tareaRepository.ObtenerPorId(id);

        if (tarea == null)
        {
            return null;
        }

        return ConvertirADto(tarea);
    }

    public TareaResponseDto Crear(CrearTareaDto crearTareaDto)
    {
        Tarea tarea = CrearTareaDesdeDto(crearTareaDto);

        _tareaRepository.Agregar(tarea);

        return ConvertirADto(tarea);
    }

    public bool Actualizar(Guid id, ActualizarTareaDto actualizarTareaDto)
    {
        Tarea? tarea = _tareaRepository.ObtenerPorId(id);

        if (tarea == null)
        {
            return false;
        }

        tarea.ActualizarDatos(
            actualizarTareaDto.Titulo,
            actualizarTareaDto.FechaLimite,
            actualizarTareaDto.Prioridad,
            actualizarTareaDto.Estado,
            actualizarTareaDto.Pilar);

        if (tarea is TareaProfunda tareaProfunda && actualizarTareaDto.Intencion != null)
        {
            tareaProfunda.CambiarIntencion(actualizarTareaDto.Intencion);
        }

        _tareaRepository.Actualizar(tarea);

        return true;
    }

    public bool Eliminar(Guid id)
    {
        return _tareaRepository.Eliminar(id);
    }

    public bool Completar(Guid id)
    {
        Tarea? tarea = _tareaRepository.ObtenerPorId(id);

        if (tarea == null)
        {
            return false;
        }

        tarea.Completar();

        _tareaRepository.Actualizar(tarea);

        return true;
    }

    private static Tarea CrearTareaDesdeDto(CrearTareaDto crearTareaDto)
    {
        return crearTareaDto.Tipo switch
        {
            TipoTarea.Simple => new TareaSimple(
                crearTareaDto.Titulo,
                crearTareaDto.FechaLimite,
                crearTareaDto.Prioridad,
                crearTareaDto.Pilar),

            TipoTarea.Urgente => new TareaUrgente(
                crearTareaDto.Titulo,
                crearTareaDto.FechaLimite,
                crearTareaDto.Pilar),

            TipoTarea.Profunda => new TareaProfunda(
                crearTareaDto.Titulo,
                crearTareaDto.FechaLimite,
                crearTareaDto.Prioridad,
                crearTareaDto.Pilar,
                crearTareaDto.Intencion ?? string.Empty),

            _ => throw new ArgumentException("Tipo de tarea no válido.")
        };
    }

    private static TareaResponseDto ConvertirADto(Tarea tarea)
    {
        return new TareaResponseDto
        {
            Id = tarea.Id,
            Titulo = tarea.Titulo,
            FechaLimite = tarea.FechaLimite,
            Prioridad = tarea.Prioridad,
            Estado = tarea.Estado,
            Pilar = tarea.Pilar,
            Tipo = ObtenerTipoTarea(tarea),
            Intencion = tarea is TareaProfunda tareaProfunda ? tareaProfunda.Intencion : null,
            Resumen = tarea.ObtenerResumen()
        };
    }

    private static TipoTarea ObtenerTipoTarea(Tarea tarea)
    {
        return tarea switch
        {
            TareaSimple => TipoTarea.Simple,
            TareaUrgente => TipoTarea.Urgente,
            TareaProfunda => TipoTarea.Profunda,
            _ => throw new ArgumentException("Tipo de tarea no reconocido.")
        };
    }
}