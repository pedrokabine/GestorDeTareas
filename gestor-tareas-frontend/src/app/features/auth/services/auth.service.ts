import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { LoginUsuarioDto } from '../models/login-usuario.dto';
import { RegistroUsuarioDto } from '../models/registro-usuario.dto';
import { AuthResponse } from '../models/auth-response.model';
import { UsuarioResponse } from '../models/usuario-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5269/api/auth';
  private tokenKey = 'token';

  constructor(private http: HttpClient) {}

  registrar(registro: RegistroUsuarioDto): Observable<UsuarioResponse> {
    return this.http.post<UsuarioResponse>(`${this.apiUrl}/registro`, registro);
  }

  login(login: LoginUsuarioDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, login);
  }

  guardarToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  obtenerToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  cerrarSesion(): void {
    localStorage.removeItem(this.tokenKey);
  }

  estaAutenticado(): boolean {
    return this.obtenerToken() !== null;
  }
}