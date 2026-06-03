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
        ValidarTitulo(titulo);
        ValidarFechaLimite(fechaLimite);

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

    public virtual void ActualizarDatos(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        EstadoTarea estado,
        PilarVida pilar)
    {
        ValidarTitulo(titulo);
        ValidarFechaLimite(fechaLimite);

        Titulo = titulo.Trim();
        FechaLimite = fechaLimite;
        Prioridad = prioridad;
        Estado = estado;
        Pilar = pilar;
    }

    public abstract string ObtenerResumen();

    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El título de la tarea es obligatorio.");
        }
    }

    private static void ValidarFechaLimite(DateTime fechaLimite)
    {
        if (fechaLimite.Date < DateTime.Today)
        {
            throw new ArgumentException("La fecha límite no puede ser anterior a hoy.");
        }
    }
}