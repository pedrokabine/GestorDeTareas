using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Domain.Entities;

public class TareaSimple : Tarea
{
    public TareaSimple()
    {
    }

    public TareaSimple(string titulo, DateTime fechaLimite, PrioridadTarea prioridad, PilarVida pilar)
        : base(titulo, fechaLimite, prioridad, pilar)
    {
    }

    public override string ObtenerResumen()
    {
        return $"Tarea simple: {Titulo}. Prioridad: {Prioridad}. Pilar: {Pilar}.";
    }
}