using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Domain.Entities;

public class TareaProfunda : Tarea
{
    public string Intencion { get; private set; } = string.Empty;

    public TareaProfunda()
    {
    }

    public TareaProfunda(
        string titulo,
        DateTime fechaLimite,
        PrioridadTarea prioridad,
        PilarVida pilar,
        string intencion)
        : base(titulo, fechaLimite, prioridad, pilar)
    {
        CambiarIntencion(intencion);
    }

    public void CambiarIntencion(string intencion)
    {
        if (string.IsNullOrWhiteSpace(intencion))
        {
            throw new ArgumentException("La intención de una tarea profunda es obligatoria.");
        }

        Intencion = intencion.Trim();
    }

    public override string ObtenerResumen()
    {
        return $"Tarea profunda: {Titulo}. Intención: {Intencion}. Pilar: {Pilar}.";
    }
}