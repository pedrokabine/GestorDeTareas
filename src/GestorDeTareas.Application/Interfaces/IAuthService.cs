using GestorDeTareas.Application.DTOs.Auth;

namespace GestorDeTareas.Application.Interfaces;

public interface IAuthService
{
    UsuarioResponseDto Registrar(RegistroUsuarioDto registroUsuarioDto);
    AuthResponseDto Login(LoginUsuarioDto loginUsuarioDto);
}