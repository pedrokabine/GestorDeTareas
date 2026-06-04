using GestorDeTareas.Application.Interfaces;
using GestorDeTareas.Domain.Entities;
using GestorDeTareas.Infrastructure.Data;

namespace GestorDeTareas.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly GestorDeTareasDbContext _context;

    public UsuarioRepository(GestorDeTareasDbContext context)
    {
        _context = context;
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        return _context.Usuarios
            .FirstOrDefault(usuario => usuario.Email == email.Trim().ToLower());
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios
            .FirstOrDefault(usuario => usuario.Id == id);
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public bool ExisteEmail(string email)
    {
        return _context.Usuarios
            .Any(usuario => usuario.Email == email.Trim().ToLower());
    }
}