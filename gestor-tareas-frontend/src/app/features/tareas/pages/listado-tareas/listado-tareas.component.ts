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

        this.tareaForm.reset({
          titulo: '',
          fechaLimite: '',
          prioridad: PrioridadTarea.Media,
          pilar: PilarVida.Estudios,
          tipo: TipoTarea.Simple,
          intencion: ''
        });

        this.changeDetector.detectChanges();
      },
      error: () => {
        this.error = 'No se ha podido crear la tarea.';
        this.changeDetector.detectChanges();
      }
    });
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
    return PilarVida[pilar];
  }

  obtenerTipo(tipo: TipoTarea): string {
    return TipoTarea[tipo];
  }
}