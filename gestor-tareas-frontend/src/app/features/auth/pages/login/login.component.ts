import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService } from '../../services/auth.service';
import { LoginUsuarioDto } from '../../models/login-usuario.dto';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  mensaje = '';
  error = '';

  loginForm;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
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
        this.router.navigate(['/tareas']);
      },
      error: () => {
        this.error = 'Email o contraseña incorrectos.';
      }
    });
  }
}