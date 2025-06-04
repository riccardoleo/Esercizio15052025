import { Routes } from '@angular/router';
import { LoginComponent } from '../pages/login/login.component';
import { DashboardComponent } from '../pages/dashboard/dashboard.component';
import { authGuard } from './guards/auth.guard'; 
import { PlantcomponentComponent } from '../pages/plantcomponent/plantcomponent.component';
import { ToolComponent } from '../pages/tool/tool.component';
import { ToolCategoryComponent } from '../pages/toolcategory/toolcategory.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard]
  },
  {
    path: 'plant-component',
    component: PlantcomponentComponent,
    canActivate: [authGuard] 
  },
  {
    path: 'tool',
    component: ToolComponent,
    canActivate: [authGuard] 
  },
  {
    path: 'tool-category',
    component: ToolCategoryComponent,
    canActivate: [authGuard] 
  },
  {
    path: '**', 
    redirectTo: '/login'
  }
];