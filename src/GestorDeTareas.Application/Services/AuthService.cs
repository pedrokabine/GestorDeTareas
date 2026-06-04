using GestorDeTareas.Application.DTOs.Auth;
using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
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
}