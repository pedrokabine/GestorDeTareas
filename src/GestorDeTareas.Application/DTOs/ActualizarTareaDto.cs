using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Application.DTOs;

public class ActualizarTareaDto
{
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public EstadoTarea Estado { get; set; }
    public PilarVida Pilar { get; set; }

    public string? Intencion { get; set; }
}