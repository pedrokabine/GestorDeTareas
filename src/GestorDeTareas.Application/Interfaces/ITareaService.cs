using GestorDeTareas.Application.DTOs;

namespace GestorDeTareas.Application.Interfaces;

public interface ITareaService
{
    List<TareaResponseDto> ObtenerTodas(Guid usuarioId);
    TareaResponseDto? ObtenerPorId(Guid id, Guid usuarioId);
    TareaResponseDto Crear(CrearTareaDto crearTareaDto, Guid usuarioId);
    bool Actualizar(Guid id, ActualizarTareaDto actualizarTareaDto, Guid usuarioId);
    bool Eliminar(Guid id, Guid usuarioId);
    bool Completar(Guid id, Guid usuarioId);
}