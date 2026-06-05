using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Infrastructure.Repositories;

public class TareaRepositoryEnMemoria : ITareaRepository
{
    private readonly List<Tarea> _tareas = new();

    public List<Tarea> ObtenerTodas(Guid usuarioId)
    {
        return _tareas
            .Where(tarea => tarea.UsuarioId == usuarioId)
            .ToList();
    }

    public Tarea? ObtenerPorId(Guid id, Guid usuarioId)
    {
        return _tareas.FirstOrDefault(tarea =>
            tarea.Id == id && tarea.UsuarioId == usuarioId);
    }

    public void Agregar(Tarea tarea)
    {
        _tareas.Add(tarea);
    }

    public void Actualizar(Tarea tarea)
    {
        int indice = _tareas.FindIndex(t => t.Id == tarea.Id);

        if (indice >= 0)
        {
            _tareas[indice] = tarea;
        }
    }

    public bool Eliminar(Guid id, Guid usuarioId)
    {
        Tarea? tarea = ObtenerPorId(id, usuarioId);

        if (tarea == null)
        {
            return false;
        }

        _tareas.Remove(tarea);
        return true;
    }
}