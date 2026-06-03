using GestorDeTareas.Domain.Entities;
using GestorDeTareas.Domain.Enums;

namespace GestorDeTareas.Tests.Domain;

[TestFixture]
public class TareaTests
{
    //Comentario para Fran: Se que es mala práctica tener los comentarios, pero te he marcado las partes de arrange, act y assert para que te sea más fácil entender la estructura de cada test y también para teneerlo yo claro.
    [Test]
    public void TareaSimple_DatosValidos_CreaTareaPendiente()
    {
        // Arrange
        string titulo = "Estudiar Angular";
        DateTime fechaLimite = DateTime.Today.AddDays(2);

        // Act
        TareaSimple tarea = new TareaSimple(
            titulo,
            fechaLimite,
            PrioridadTarea.Media,
            PilarVida.Estudios);

        // Assert
        Assert.That(tarea.Titulo, Is.EqualTo(titulo));
        Assert.That(tarea.FechaLimite, Is.EqualTo(fechaLimite));
        Assert.That(tarea.Prioridad, Is.EqualTo(PrioridadTarea.Media));
        Assert.That(tarea.Pilar, Is.EqualTo(PilarVida.Estudios));
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Pendiente));
        Assert.That(tarea.Id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void TareaSimple_TituloVacio_LanzaExcepcion()
    {
        // Arrange
        string titulo = "";
        DateTime fechaLimite = DateTime.Today.AddDays(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new TareaSimple(
                titulo,
                fechaLimite,
                PrioridadTarea.Baja,
                PilarVida.Trabajo));
    }

    [Test]
    public void TareaSimple_FechaPasada_LanzaExcepcion()
    {
        // Arrange
        string titulo = "Tarea con fecha pasada";
        DateTime fechaLimite = DateTime.Today.AddDays(-1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new TareaSimple(
                titulo,
                fechaLimite,
                PrioridadTarea.Alta,
                PilarVida.Estudios));
    }

    [Test]
    public void Completar_TareaPendiente_CambiaEstadoACompletada()
    {
        // Arrange
        TareaSimple tarea = new TareaSimple(
            "Entrenar",
            DateTime.Today.AddDays(1),
            PrioridadTarea.Media,
            PilarVida.Salud);

        // Act
        tarea.Completar();

        // Assert
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Completada));
    }

    [Test]
    public void Completar_TareaYaCompletada_LanzaExcepcion()
    {
        // Arrange
        TareaSimple tarea = new TareaSimple(
            "Leer documentación",
            DateTime.Today.AddDays(1),
            PrioridadTarea.Media,
            PilarVida.Estudios);

        tarea.Completar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }

    [Test]
    public void Completar_TareaCancelada_LanzaExcepcion()
    {
        // Arrange
        TareaSimple tarea = new TareaSimple(
            "Tarea cancelada",
            DateTime.Today.AddDays(1),
            PrioridadTarea.Baja,
            PilarVida.Trabajo);

        tarea.CambiarEstado(EstadoTarea.Cancelada);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }

    [Test]
    public void CambiarEstado_NuevoEstado_CambiaCorrectamente()
    {
        // Arrange
        TareaSimple tarea = new TareaSimple(
            "Preparar proyecto",
            DateTime.Today.AddDays(3),
            PrioridadTarea.Alta,
            PilarVida.Estudios);

        // Act
        tarea.CambiarEstado(EstadoTarea.EnProgreso);

        // Assert
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.EnProgreso));
    }

    [Test]
    public void TareaUrgente_DatosValidos_AsignaPrioridadUrgente()
    {
        // Arrange
        string titulo = "Entregar proyecto";
        DateTime fechaLimite = DateTime.Today.AddDays(1);

        // Act
        TareaUrgente tarea = new TareaUrgente(
            titulo,
            fechaLimite,
            PilarVida.Estudios);

        // Assert
        Assert.That(tarea.Prioridad, Is.EqualTo(PrioridadTarea.Urgente));
        Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Pendiente));
    }

    [Test]
    public void TareaProfunda_DatosValidos_GuardaIntencion()
    {
        // Arrange
        string intencion = "Avanzar con calma y constancia";

        // Act
        TareaProfunda tarea = new TareaProfunda(
            "Reflexionar sobre la semana",
            DateTime.Today.AddDays(2),
            PrioridadTarea.Media,
            PilarVida.DesarrolloPersonal,
            intencion);

        // Assert
        Assert.That(tarea.Intencion, Is.EqualTo(intencion));
        Assert.That(tarea.Pilar, Is.EqualTo(PilarVida.DesarrolloPersonal));
    }

    [Test]
    public void TareaProfunda_IntencionVacia_LanzaExcepcion()
    {
        // Arrange
        string intencion = "";

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new TareaProfunda(
                "Meditar",
                DateTime.Today.AddDays(1),
                PrioridadTarea.Media,
                PilarVida.DesarrolloPersonal,
                intencion));
    }

    [Test]
    public void ObtenerResumen_DiferentesTiposDeTarea_AplicaPolimorfismo()
    {
        // Arrange
        List<Tarea> tareas = new List<Tarea>
        {
            new TareaSimple(
                "Comprar material",
                DateTime.Today.AddDays(1),
                PrioridadTarea.Baja,
                PilarVida.Trabajo),

            new TareaUrgente(
                "Entregar práctica",
                DateTime.Today.AddDays(1),
                PilarVida.Estudios),

            new TareaProfunda(
                "Escribir reflexión",
                DateTime.Today.AddDays(2),
                PrioridadTarea.Media,
                PilarVida.DesarrolloPersonal,
                "Pensar en lo importante")
        };

        // Act
        List<string> resumenes = tareas
            .Select(tarea => tarea.ObtenerResumen())
            .ToList();

        // Assert
        Assert.That(resumenes[0], Does.Contain("Tarea simple"));
        Assert.That(resumenes[1], Does.Contain("Tarea urgente"));
        Assert.That(resumenes[2], Does.Contain("Tarea profunda"));
    }
}