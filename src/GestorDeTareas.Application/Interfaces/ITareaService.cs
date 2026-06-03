using GestorDeTareas.Application.DTOs;

namespace GestorDeTareas.Application.Interfaces;

public interface ITareaService
{
    List<TareaResponseDto> ObtenerTodas();
    TareaResponseDto? ObtenerPorId(Guid id);
    TareaResponseDto Crear(CrearTareaDto crearTareaDto);
    bool Actualizar(Guid id, ActualizarTareaDto actualizarTareaDto);
    bool Eliminar(Guid id);
    bool Completar(Guid id);
}