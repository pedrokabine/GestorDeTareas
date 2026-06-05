import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService } from '../../services/auth.service';
import { LoginUsuarioDto } from '../../models/login-usuario.dto';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  mensaje = '';
  error = '';

  loginForm;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  iniciarSesion(): void {
    this.mensaje = '';
    this.error = '';

    if (this.loginForm.invalid) {
      this.error = 'Introduce un email y una contraseña válidos.';
      return;
    }

    const login: LoginUsuarioDto = {
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    };

    this.authService.login(login).subscribe({
      next: (respuesta) => {
        this.authService.guardarToken(respuesta.token);
        this.mensaje = `Sesión iniciada correctamente. Bienvenido, ${respuesta.nombre}.`;
      },
      error: () => {
        this.error = 'Email o contraseña incorrectos.';
      }
    });
  }
}