import { PilarVida } from './pilar-vida.enum';
import { PrioridadTarea } from './prioridad-tarea.enum';
import { TipoTarea } from './tipo-tarea.enum';

export interface CrearTareaDto {
  titulo: string;
  fechaLimite: string;
  prioridad: PrioridadTarea;
  pilar: PilarVida;
  tipo: TipoTarea;
  intencion?: string | null;
}