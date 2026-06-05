import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Tarea } from '../models/tarea.model';
import { CrearTareaDto } from '../models/crear-tarea.dto';
import { ActualizarTareaDto } from '../models/actualizar-tarea.dto';

@Injectable({
  providedIn: 'root'
})
export class TareaService {
  private apiUrl = 'http://localhost:5269/api/tareas';

  constructor(private http: HttpClient) {}

  obtenerTodas(): Observable<Tarea[]> {
    return this.http.get<Tarea[]>(this.apiUrl, {
      headers: this.crearHeaders()
    });
  }

  obtenerPorId(id: string): Observable<Tarea> {
    return this.http.get<Tarea>(`${this.apiUrl}/${id}`, {
      headers: this.crearHeaders()
    });
  }

  crear(tarea: CrearTareaDto): Observable<Tarea> {
    return this.http.post<Tarea>(this.apiUrl, tarea, {
      headers: this.crearHeaders()
    });
  }

  actualizar(id: string, tarea: ActualizarTareaDto): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, tarea, {
      headers: this.crearHeaders()
    });
  }

  completar(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/completar`, {}, {
      headers: this.crearHeaders()
    });
  }

  eliminar(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {
      headers: this.crearHeaders()
    });
  }

  private crearHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }
}