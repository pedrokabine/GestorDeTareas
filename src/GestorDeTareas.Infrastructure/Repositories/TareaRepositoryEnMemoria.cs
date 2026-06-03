using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Infrastructure.Repositories;

public class TareaRepositoryEnMemoria : ITareaRepository
{
    private readonly List<Tarea> _tareas = new();

    public List<Tarea> ObtenerTodas()
    {
        return _tareas.ToList();
    }

    public Tarea? ObtenerPorId(Guid id)
    {
        return _tareas.FirstOrDefault(tarea => tarea.Id == id);
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

    public bool Eliminar(Guid id)
    {
        Tarea? tarea = ObtenerPorId(id);

        if (tarea == null)
        {
            return false;
        }

        _tareas.Remove(tarea);
        return true;
    }
}