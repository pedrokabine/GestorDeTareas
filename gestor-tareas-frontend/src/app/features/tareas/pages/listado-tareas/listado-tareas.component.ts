import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TareaService } from '../../services/tarea.service';
import { Tarea } from '../../models/tarea.model';
import { EstadoTarea } from '../../models/estado-tarea.enum';
import { PrioridadTarea } from '../../models/prioridad-tarea.enum';
import { PilarVida } from '../../models/pilar-vida.enum';
import { TipoTarea } from '../../models/tipo-tarea.enum';
import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-listado-tareas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './listado-tareas.component.html',
  styleUrl: './listado-tareas.component.css'
})
export class ListadoTareasComponent implements OnInit {
  tareas: Tarea[] = [];
  error = '';
  cargando = false;

  constructor(
    private tareaService: TareaService,
    private authService: AuthService,
    private router: Router,
    private changeDetector: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.cargarTareas();
  }

  cargarTareas(): void {
    this.cargando = true;
    this.error = '';
    this.changeDetector.detectChanges();

    this.tareaService.obtenerTodas().subscribe({
      next: (tareas) => {
        this.tareas = tareas;
        this.cargando = false;
        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.log('Error al cargar tareas:', error);
        this.error = 'No se han podido cargar las tareas.';
        this.cargando = false;
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
      error: (error) => {
        console.log('Error al completar tarea:', error);
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
      error: (error) => {
        console.log('Error al eliminar tarea:', error);
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