using GestorDeTareas.Application.DTOs.Auth;
using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public UsuarioResponseDto Registrar(RegistroUsuarioDto registroUsuarioDto)
    {
        if (string.IsNullOrWhiteSpace(registroUsuarioDto.Password))
        {
            throw new ArgumentException("La contraseña es obligatoria.");
        }

        if (registroUsuarioDto.Password.Length < 6)
        {
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
        }

        if (_usuarioRepository.ExisteEmail(registroUsuarioDto.Email))
        {
            throw new InvalidOperationException("Ya existe un usuario con ese email.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registroUsuarioDto.Password);

        Usuario usuario = new Usuario(
            registroUsuarioDto.Nombre,
            registroUsuarioDto.Email,
            passwordHash);

        _usuarioRepository.Agregar(usuario);

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email
        };
    }

    public AuthResponseDto Login(LoginUsuarioDto loginUsuarioDto)
    {
        Usuario? usuario = _usuarioRepository.ObtenerPorEmail(loginUsuarioDto.Email);

        if (usuario == null)
        {
            throw new InvalidOperationException("Email o contraseña incorrectos.");
        }

        bool passwordCorrecta = BCrypt.Net.BCrypt.Verify(
            loginUsuarioDto.Password,
            usuario.PasswordHash);

        if (!passwordCorrecta)
        {
            throw new InvalidOperationException("Email o contraseña incorrectos.");
        }

        string token = _tokenService.GenerarToken(usuario);

        return new AuthResponseDto
        {
            UsuarioId = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Token = token
        };
    }
}