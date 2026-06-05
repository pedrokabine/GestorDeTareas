using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;
using GestorDeTareas.Infrastructure.Data;

namespace GestorDeTareas.Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly GestorDeTareasDbContext _context;

    public TareaRepository(GestorDeTareasDbContext context)
    {
        _context = context;
    }

    public List<Tarea> ObtenerTodas(Guid usuarioId)
    {
        return _context.Tareas
            .Where(tarea => tarea.UsuarioId == usuarioId)
            .OrderBy(tarea => tarea.FechaLimite)
            .ToList();
    }

    public Tarea? ObtenerPorId(Guid id, Guid usuarioId)
    {
        return _context.Tareas
            .FirstOrDefault(tarea => tarea.Id == id && tarea.UsuarioId == usuarioId);
    }

    public void Agregar(Tarea tarea)
    {
        _context.Tareas.Add(tarea);
        _context.SaveChanges();
    }

    public void Actualizar(Tarea tarea)
    {
        _context.Tareas.Update(tarea);
        _context.SaveChanges();
    }

    public bool Eliminar(Guid id, Guid usuarioId)
    {
        Tarea? tarea = ObtenerPorId(id, usuarioId);

        if (tarea == null)
        {
            return false;
        }

        _context.Tareas.Remove(tarea);
        _context.SaveChanges();

        return true;
    }
}