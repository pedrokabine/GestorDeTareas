namespace GestorDeTareas.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime FechaRegistro { get; private set; }

    private Usuario()
    {
    }

    public Usuario(string nombre, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("El email del usuario es obligatorio.");
        }

        if (!email.Contains("@"))
        {
            throw new ArgumentException("El email no tiene un formato válido.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("La contraseña cifrada es obligatoria.");
        }

        Id = Guid.NewGuid();
        Nombre = nombre.Trim();
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;
        FechaRegistro = DateTime.Now;
    }
}