import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/pages/login/login.component';
import { RegistroComponent } from './features/auth/pages/registro/registro.component';
import { ListadoTareasComponent } from './features/tareas/pages/listado-tareas/listado-tareas.component';
import { authGuard } from './features/auth/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'registro',
    component: RegistroComponent
  },
 {
  path: 'tareas',
  component: ListadoTareasComponent,
  canActivate: [authGuard]
},
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];