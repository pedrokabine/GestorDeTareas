using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Interfaces;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}