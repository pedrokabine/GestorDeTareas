using GestorDeTareas.Application.DTOs.Auth;
using GestorDeTareas.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeTareas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registro")]
    public ActionResult<UsuarioResponseDto> Registrar(RegistroUsuarioDto registroUsuarioDto)
    {
        try
        {
            UsuarioResponseDto usuario = _authService.Registrar(registroUsuarioDto);

            return CreatedAtAction(
                nameof(Registrar),
                new { id = usuario.Id },
                usuario);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login(LoginUsuarioDto loginUsuarioDto)
    {
        try
        {
            AuthResponseDto respuesta = _authService.Login(loginUsuarioDto);

            return Ok(respuesta);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}