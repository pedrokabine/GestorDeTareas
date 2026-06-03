using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Interfaces;

public interface ITareaRepository
{
    List<Tarea> ObtenerTodas();
    Tarea? ObtenerPorId(Guid id);
    void Agregar(Tarea tarea);
    void Actualizar(Tarea tarea);
    bool Eliminar(Guid id);
}