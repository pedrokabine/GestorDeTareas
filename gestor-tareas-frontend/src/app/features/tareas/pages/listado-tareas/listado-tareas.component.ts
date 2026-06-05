import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TareaService } from '../../services/tarea.service';
import { Tarea } from '../../models/tarea.model';
import { EstadoTarea } from '../../models/estado-tarea.enum';
import { PrioridadTarea } from '../../models/prioridad-tarea.enum';
import { PilarVida } from '../../models/pilar-vida.enum';
import { TipoTarea } from '../../models/tipo-tarea.enum';
import { AuthService } from '../../../auth/services/auth.service';
import { CrearTareaDto } from '../../models/crear-tarea.dto';
import { ActualizarTareaDto } from '../../models/actualizar-tarea.dto';

@Component({
  selector: 'app-listado-tareas',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './listado-tareas.component.html',
  styleUrl: './listado-tareas.component.css'
})
export class ListadoTareasComponent implements OnInit {
  tareas: Tarea[] = [];
  error = '';
  mensaje = '';
  cargando = false;

  tareaEditandoId: string | null = null;
  estadoTareaEditando: EstadoTarea = EstadoTarea.Pendiente;

  prioridades = [
    { valor: PrioridadTarea.Baja, texto: 'Baja' },
    { valor: PrioridadTarea.Media, texto: 'Media' },
    { valor: PrioridadTarea.Alta, texto: 'Alta' },
    { valor: PrioridadTarea.Urgente, texto: 'Urgente' }
  ];

  pilares = [
    { valor: PilarVida.Salud, texto: 'Salud' },
    { valor: PilarVida.Estudios, texto: 'Estudios' },
    { valor: PilarVida.Trabajo, texto: 'Trabajo' },
    { valor: PilarVida.Familia, texto: 'Familia' },
    { valor: PilarVida.DesarrolloPersonal, texto: 'Desarrollo personal' },
    { valor: PilarVida.Relaciones, texto: 'Relaciones' },
    { valor: PilarVida.Finanzas, texto: 'Finanzas' }
  ];

  tipos = [
    { valor: TipoTarea.Simple, texto: 'Simple' },
    { valor: TipoTarea.Urgente, texto: 'Urgente' },
    { valor: TipoTarea.Profunda, texto: 'Profunda' }
  ];

  tareaForm;

  constructor(
    private tareaService: TareaService,
    private authService: AuthService,
    private router: Router,
    private changeDetector: ChangeDetectorRef,
    private formBuilder: FormBuilder
  ) {
    this.tareaForm = this.formBuilder.group({
      titulo: ['', [Validators.required]],
      fechaLimite: ['', [Validators.required]],
      prioridad: [PrioridadTarea.Media, [Validators.required]],
      pilar: [PilarVida.Estudios, [Validators.required]],
      tipo: [TipoTarea.Simple, [Validators.required]],
      intencion: ['']
    });
  }

  ngOnInit(): void {
    this.cargarTareas();
  }

  cargarTareas(): void {
    this.cargando = true;
    this.error = '';

    this.tareaService.obtenerTodas().subscribe({
      next: (tareas) => {
        this.tareas = tareas;
        this.cargando = false;
        this.changeDetector.detectChanges();
      },
      error: () => {
        this.error = 'No se han podido cargar las tareas.';
        this.cargando = false;
        this.changeDetector.detectChanges();
      }
    });
  }

  guardarTarea(): void {
    if (this.tareaEditandoId === null) {
      this.crearTarea();
    } else {
      this.actualizarTarea();
    }
  }

  crearTarea(): void {
    this.error = '';
    this.mensaje = '';

    if (this.tareaForm.invalid) {
      this.error = 'Rellena los campos obligatorios.';
      return;
    }

    const tipo = Number(this.tareaForm.value.tipo);
    const intencion = this.tareaForm.value.intencion ?? '';

    const nuevaTarea: CrearTareaDto = {
      titulo: this.tareaForm.value.titulo ?? '',
      fechaLimite: this.tareaForm.value.fechaLimite ?? '',
      prioridad: Number(this.tareaForm.value.prioridad),
      pilar: Number(this.tareaForm.value.pilar),
      tipo: tipo,
      intencion: tipo === TipoTarea.Profunda ? intencion : null
    };

    this.tareaService.crear(nuevaTarea).subscribe({
      next: (tareaCreada) => {
        this.tareas = [...this.tareas, tareaCreada];
        this.mensaje = 'Tarea creada correctamente.';
        this.limpiarFormulario();
        this.changeDetector.detectChanges();
      },
      error: () => {
        this.error = 'No se ha podido crear la tarea.';
        this.changeDetector.detectChanges();
      }
    });
  }

  iniciarEdicion(tarea: Tarea): void {
  this.tareaEditandoId = tarea.id;
  this.estadoTareaEditando = tarea.estado;
  this.mensaje = 'Editando tarea. Modifica los datos en el formulario superior.';
  this.error = '';

  this.tareaForm.patchValue({
    titulo: tarea.titulo,
    fechaLimite: tarea.fechaLimite.substring(0, 10),
    prioridad: tarea.prioridad,
    pilar: tarea.pilar,
    tipo: tarea.tipo,
    intencion: tarea.intencion ?? ''
  });

  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  });

  this.changeDetector.detectChanges();
}

  actualizarTarea(): void {
    this.error = '';
    this.mensaje = '';

    if (this.tareaEditandoId === null) {
      return;
    }

    if (this.tareaForm.invalid) {
      this.error = 'Rellena los campos obligatorios.';
      return;
    }

    const tareaActualizada: ActualizarTareaDto = {
      titulo: this.tareaForm.value.titulo ?? '',
      fechaLimite: this.tareaForm.value.fechaLimite ?? '',
      prioridad: Number(this.tareaForm.value.prioridad),
      estado: this.estadoTareaEditando,
      pilar: Number(this.tareaForm.value.pilar),
      intencion: this.tareaForm.value.intencion ?? null
    };

    this.tareaService.actualizar(this.tareaEditandoId, tareaActualizada).subscribe({
      next: () => {
        this.mensaje = 'Tarea actualizada correctamente.';
        this.tareaEditandoId = null;
        this.limpiarFormulario();
        this.cargarTareas();
      },
      error: () => {
        this.error = 'No se ha podido actualizar la tarea.';
        this.changeDetector.detectChanges();
      }
    });
  }

  cancelarEdicion(): void {
    this.tareaEditandoId = null;
    this.estadoTareaEditando = EstadoTarea.Pendiente;
    this.limpiarFormulario();
    this.mensaje = '';
    this.error = '';
    this.changeDetector.detectChanges();
  }

  completarTarea(id: string): void {
    this.error = '';

    this.tareaService.completar(id).subscribe({
      next: () => {
        this.cargarTareas();
      },
      error: () => {
        this.error = 'No se ha podido completar la tarea.';
        this.changeDetector.detectChanges();
      }
    });
  }

  eliminarTarea(id: string): void {
    this.error = '';

    this.tareaService.eliminar(id).subscribe({
      next: () => {
        this.tareas = this.tareas.filter(tarea => tarea.id !== id);
        this.changeDetector.detectChanges();
      },
      error: () => {
        this.error = 'No se ha podido eliminar la tarea.';
        this.changeDetector.detectChanges();
      }
    });
  }

  limpiarFormulario(): void {
    this.tareaForm.reset({
      titulo: '',
      fechaLimite: '',
      prioridad: PrioridadTarea.Media,
      pilar: PilarVida.Estudios,
      tipo: TipoTarea.Simple,
      intencion: ''
    });
  }

  cerrarSesion(): void {
    this.authService.cerrarSesion();
    this.router.navigate(['/login']);
  }

  obtenerEstado(estado: EstadoTarea): string {
    return EstadoTarea[estado];
  }

  obtenerPrioridad(prioridad: PrioridadTarea): string {
    return PrioridadTarea[prioridad];
  }

 obtenerPilar(pilar: PilarVida): string {
  switch (pilar) {
    case PilarVida.Salud:
      return 'Salud';
    case PilarVida.Estudios:
      return 'Estudios';
    case PilarVida.Trabajo:
      return 'Trabajo';
    case PilarVida.Familia:
      return 'Familia';
    case PilarVida.DesarrolloPersonal:
      return 'Desarrollo personal';
    case PilarVida.Relaciones:
      return 'Relaciones';
    case PilarVida.Finanzas:
      return 'Finanzas';
    default:
      return 'Sin pilar';
  }
}
  obtenerTipo(tipo: TipoTarea): string {
    return TipoTarea[tipo];
  }

  obtenerResumen(tarea: Tarea): string {
  if (tarea.tipo === TipoTarea.Profunda) {
    return `Tarea profunda: ${tarea.titulo}. Intención: ${tarea.intencion}. Pilar: ${this.obtenerPilar(tarea.pilar)}.`;
  }

  if (tarea.tipo === TipoTarea.Urgente) {
    return `Tarea urgente: ${tarea.titulo}. Fecha límite: ${new Date(tarea.fechaLimite).toLocaleDateString('es-ES')}. Pilar: ${this.obtenerPilar(tarea.pilar)}.`;
  }

  return `Tarea simple: ${tarea.titulo}. Prioridad: ${this.obtenerPrioridad(tarea.prioridad)}. Pilar: ${this.obtenerPilar(tarea.pilar)}.`;
}
}