import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

// Interfaccia per la risposta del backend (stessa del login)
interface UserResponseDTO {
  success: number;
  UserId?: number;
  user_DTO?: any;
  users?: User[];
  token?: string;
  message?: string;
  UserRole?: string;
  IsAdmin?: boolean;
}

// Interfaccia per l'utente
interface User {
  userId: number;
  username: string;
  email: string;
  role: string;
  creationDate: Date;
  isActive?: boolean;
}

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  
  private router = inject(Router);
  private http = inject(HttpClient);
  
  // --- Configurazione API ---
  private readonly apiUrl = 'https://localhost:7121/user'; // Adatta questo URL alla tua API degli utenti
  
  userRole: string | null = null;
  userName: string | null = null;
  token: string = '';
  isLoading: boolean = true;
  errorMessage: string | null = null;

  // --- Gestione lista utenti ---
  showUsersList: boolean = false;
  users: User[] = [];
  loadingUsers: boolean = false;
  totalUsersCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  ngOnInit() {
    this.loadUserData();
  }

  loadUserData() {
    debugger;
    const token = localStorage.getItem('authToken');
    const userRole = localStorage.getItem('userRole');
    const userName = localStorage.getItem('userName');
    
    if (!token) {
      console.error('Nessun token trovato');
      this.router.navigate(['/login']);
      return;
    }

    if (!userRole) {
      console.error('Nessun ruolo utente trovato');
      this.errorMessage = 'Dati utente non completi';
      this.isLoading = false;
      return;
    }

    this.token = token;

    // Usa prima il nome dal localStorage, poi prova dal token
    if (userName) {
      this.userName = userName;
    } else {
      // Fallback: prova a decodificare dal token
      try {
        const tokenPayload = this.decodeJwtToken(token);
        this.userName = tokenPayload?.unique_name || tokenPayload?.name || 'Utente';
      } catch (error) {
        console.warn('Impossibile decodificare il token:', error);
        this.userName = 'Utente';
      }
    }

    this.userRole = userRole;
    this.isLoading = false;
    
    console.log('Dati utente caricati:', {
      userName: this.userName,
      userRole: this.userRole
    });
  }

  // Metodo per controllare se l'utente è admin
  isAdmin(): boolean {
    return this.userRole?.toLowerCase() === 'admin';
  }

  // Metodo per decodificare il JWT token (senza librerie esterne)
  private decodeJwtToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = atob(payload);
      return JSON.parse(decodedPayload);
    } catch (error) {
      console.error('Errore nella decodifica del token:', error);
      return null;
    }
  }

  // === GESTIONE HEADERS HTTP ===
  private getHttpHeaders(): HttpHeaders {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    if (this.token) {
      headers = headers.set('Authorization', `Bearer ${this.token}`);
    } else {
      console.error('Token di autenticazione mancante!');
    }
    return headers;
  }

  private isAuthenticated(): boolean {
    return !!this.token;
  }

  private handleAuthError(): void {
    console.warn('Errore di autenticazione → logout e reindirizzamento');
    this.logout();
  }

  // === GESTIONE LISTA UTENTI ===
  viewAllUsers(): void {
    if (this.isAdmin()) {
      this.showUsersList = !this.showUsersList;
      if (this.showUsersList && this.users.length === 0) {
        this.loadUsers();
      }
      console.log('Visualizza tutti gli utenti - funzione admin');
    }
  }

  hideUsersList(): void {
    this.showUsersList = false;
  }

  loadUsers(): void {
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loadingUsers = true;
    const url = `${this.apiUrl}/getall/${this.currentPage}/${this.pageSize}`;

    console.log('Chiamata API per utenti →', url);
    console.log('Headers →', this.getHttpHeaders());

    this.http.get<UserResponseDTO>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName || '' }
    }).subscribe({
      next: (response) => {
        this.loadingUsers = false;
        console.log('Risposta API utenti →', response);

        if (response.success === 200 && response.users) {
          debugger;
          this.users = response.users;
          this.totalUsersCount = this.users.length;
          console.log('Utenti caricati:', this.users);
        } else {
          this.handleError('Errore nel caricamento degli utenti: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loadingUsers = false;
        this.handleHttpError(error);
      }
    });
  }

  // === PAGINAZIONE UTENTI ===
  getTotalUsersPages(): number {
    return Math.ceil(this.totalUsersCount / this.pageSize);
  }

  goToUsersPage(page: number): void {
    if (page >= 1 && page <= this.getTotalUsersPages()) {
      this.currentPage = page;
      this.loadUsers();
    }
  }

  // === UTILITY ===
  formatDate(date: Date | string): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString('it-IT');
  }

  private handleError(message: string): void {
    console.error(message);
    alert(message);
  }

  private handleHttpError(error: HttpErrorResponse): void {
    console.error('Errore HTTP:', error);

    let errorMessage = 'Errore di connessione al server';
    switch (error.status) {
      case 0:
        errorMessage = 'Impossibile contattare il server – verifica la connessione';
        break;
      case 401:
        errorMessage = 'Non autorizzato – token scaduto o non valido';
        this.handleAuthError();
        return;
      case 403:
        errorMessage = 'Accesso negato – permessi insufficienti';
        break;
      case 404:
        errorMessage = 'Risorsa non trovata';
        break;
      case 500:
        errorMessage = 'Errore interno del server';
        break;
      default:
        if (error.error?.message) {
          errorMessage = error.error.message;
        }
    }
    this.handleError(errorMessage);
  }

  // Metodo per effettuare il logout
  logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userRole');
    localStorage.removeItem('userName');
    this.router.navigate(['/login']);
  }

  // Metodo per ricaricare i dati (utile per il pulsante retry nell'HTML)
  reloadData() {
    this.isLoading = true;
    this.errorMessage = null;
    this.loadUserData();
  }

  // Navigazione generica
  navigateTo(route: string): void {
    this.router.navigate([route]);
  }
}