using GestorDeTareas.Domain.Entities;
using GestorDeTareas.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestorDeTareas.Infrastructure.Data;

public class GestorDeTareasDbContext : DbContext
{
    public GestorDeTareasDbContext(DbContextOptions<GestorDeTareasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tarea>()
            .HasKey(tarea => tarea.Id);

        modelBuilder.Entity<Tarea>()
            .Property(tarea => tarea.Titulo)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Tarea>()
            .Property(tarea => tarea.FechaLimite)
            .IsRequired();

        modelBuilder.Entity<Tarea>()
            .Property(tarea => tarea.Prioridad)
            .IsRequired();

        modelBuilder.Entity<Tarea>()
            .Property(tarea => tarea.Estado)
            .IsRequired();

        modelBuilder.Entity<Tarea>()
            .Property(tarea => tarea.Pilar)
            .IsRequired();

        modelBuilder.Entity<Tarea>()
            .HasDiscriminator<TipoTarea>("Tipo")
            .HasValue<TareaSimple>(TipoTarea.Simple)
            .HasValue<TareaUrgente>(TipoTarea.Urgente)
            .HasValue<TareaProfunda>(TipoTarea.Profunda);

        modelBuilder.Entity<TareaProfunda>()
            .Property(tarea => tarea.Intencion)
            .HasMaxLength(300);

        modelBuilder.Entity<Usuario>()
            .HasKey(usuario => usuario.Id);

        modelBuilder.Entity<Usuario>()
            .Property(usuario => usuario.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Usuario>()
            .Property(usuario => usuario.Email)
            .IsRequired()
            .HasMaxLength(150);

        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.Email)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .Property(usuario => usuario.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Usuario>()
            .Property(usuario => usuario.FechaRegistro)
            .IsRequired();
    }
}