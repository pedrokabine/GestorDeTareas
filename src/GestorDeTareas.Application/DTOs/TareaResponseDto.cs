using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Application.DTOs;

public class TareaResponseDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaLimite { get; set; }
    public PrioridadTarea Prioridad { get; set; }
    public EstadoTarea Estado { get; set; }
    public PilarVida Pilar { get; set; }
    public TipoTarea Tipo { get; set; }
    public string? Intencion { get; set; }
    public string Resumen { get; set; } = string.Empty;
}