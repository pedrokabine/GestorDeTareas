import { EstadoTarea } from './estado-tarea.enum';
import { PilarVida } from './pilar-vida.enum';
import { PrioridadTarea } from './prioridad-tarea.enum';

export interface ActualizarTareaDto {
  titulo: string;
  fechaLimite: string;
  prioridad: PrioridadTarea;
  estado: EstadoTarea;
  pilar: PilarVida;
  intencion?: string | null;
}