import { EstadoTarea } from './estado-tarea.enum';
import { PilarVida } from './pilar-vida.enum';
import { PrioridadTarea } from './prioridad-tarea.enum';
import { TipoTarea } from './tipo-tarea.enum';

export interface Tarea {
  id: string;
  titulo: string;
  fechaLimite: string;
  prioridad: PrioridadTarea;
  estado: EstadoTarea;
  pilar: PilarVida;
  tipo: TipoTarea;
  usuarioId?: string;
  intencion?: string | null;
  resumen: string;
}