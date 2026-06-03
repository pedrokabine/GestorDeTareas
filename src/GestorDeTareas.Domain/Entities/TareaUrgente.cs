using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Domain.Entities;

public class TareaUrgente : Tarea
{
    public TareaUrgente()
    {
    }

    public TareaUrgente(string titulo, DateTime fechaLimite, PilarVida pilar)
        : base(titulo, fechaLimite, PrioridadTarea.Urgente, pilar)
    {
    }

    public override void ActualizarDatos(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        EstadoTarea estado,
        PilarVida pilar)
    {
        base.ActualizarDatos(
            titulo,
            fechaLimite,
            PrioridadTarea.Urgente,
            estado,
            pilar);
    }

    public override string ObtenerResumen()
    {
        return $"Tarea urgente: {Titulo}. Fecha límite: {FechaLimite:dd/MM/yyyy}. Pilar: {Pilar}.";
    }
}