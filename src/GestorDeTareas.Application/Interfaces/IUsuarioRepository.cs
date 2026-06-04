using GestorDeTareas.Domain.Entities;

namespace GestorDeTareas.Application.Interfaces;

public interface IUsuarioRepository
{
    Usuario? ObtenerPorEmail(string email);
    Usuario? ObtenerPorId(Guid id);
    void Agregar(Usuario usuario);
    bool ExisteEmail(string email);
}