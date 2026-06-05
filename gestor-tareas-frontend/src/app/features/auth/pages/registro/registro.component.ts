import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { RegistroUsuarioDto } from '../../models/registro-usuario.dto';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {
  mensaje = '';
  error = '';

  registroForm;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registroForm = this.formBuilder.group({
      nombre: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  registrar(): void {
    this.mensaje = '';
    this.error = '';

    if (this.registroForm.invalid) {
      this.error = 'Rellena todos los campos correctamente.';
      return;
    }

    const registro: RegistroUsuarioDto = {
      nombre: this.registroForm.value.nombre ?? '',
      email: this.registroForm.value.email ?? '',
      password: this.registroForm.value.password ?? ''
    };

    this.authService.registrar(registro).subscribe({
      next: () => {
        this.mensaje = 'Usuario registrado correctamente. Ya puedes iniciar sesión.';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1000);
      },
      error: () => {
        this.error = 'No se ha podido registrar el usuario. Puede que el email ya exista.';
      }
    });
  }
}