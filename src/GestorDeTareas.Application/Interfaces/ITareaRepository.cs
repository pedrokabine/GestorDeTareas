using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Interfaces;

public interface ITareaRepository
{
    List<Tarea> ObtenerTodas(Guid usuarioId);
    Tarea? ObtenerPorId(Guid id, Guid usuarioId);
    void Agregar(Tarea tarea);
    void Actualizar(Tarea tarea);
    bool Eliminar(Guid id, Guid usuarioId);
}