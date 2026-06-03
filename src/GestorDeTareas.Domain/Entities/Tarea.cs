using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Domain.Entities;

public abstract class Tarea
{
    public Guid Id { get; protected set; }
    public string Titulo { get; protected set; } = string.Empty;
    public DateTime FechaLimite { get; protected set; }
    public PrioridadTarea Prioridad { get; protected set; }
    public EstadoTarea Estado { get; protected set; }
    public PilarVida Pilar { get; protected set; }

    protected Tarea()
    {
    }

    protected Tarea(string titulo, DateTime fechaLimite, PrioridadTarea prioridad, PilarVida pilar)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El título de la tarea es obligatorio.");
        }

        if (fechaLimite.Date < DateTime.Today)
        {
            throw new ArgumentException("La fecha límite no puede ser anterior a hoy.");
        }

        Id = Guid.NewGuid();
        Titulo = titulo.Trim();
        FechaLimite = fechaLimite;
        Prioridad = prioridad;
        Pilar = pilar;
        Estado = EstadoTarea.Pendiente;
    }

    public void Completar()
    {
        if (Estado == EstadoTarea.Completada)
        {
            throw new InvalidOperationException("La tarea ya está completada.");
        }

        if (Estado == EstadoTarea.Cancelada)
        {
            throw new InvalidOperationException("Una tarea cancelada no se puede completar.");
        }

        Estado = EstadoTarea.Completada;
    }

    public void CambiarEstado(EstadoTarea nuevoEstado)
    {
        Estado = nuevoEstado;
    }

    public abstract string ObtenerResumen();
}