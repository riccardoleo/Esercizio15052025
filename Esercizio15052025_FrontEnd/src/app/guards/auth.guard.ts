import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
   
  const  isLocalStorageAvailable = typeof localStorage !== 'undefined'
  if (isLocalStorageAvailable && localStorage.getItem('authToken')) {
    return true;
  }

  // Redirect a /login se non autenticato
  return router.createUrlTree(['/login']);
};

