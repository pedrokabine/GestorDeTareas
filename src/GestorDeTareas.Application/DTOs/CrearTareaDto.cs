using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Application.DTOs;

public class CrearTareaDto
{
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public PilarVida Pilar { get; set; }
    public TipoTarea Tipo { get; set; }

    public string? Intencion { get; set; }
}