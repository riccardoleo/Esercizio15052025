import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

interface PlantComponent {
  componentId: number;
  name: string;
  description: string;
  createdByUserId?: number;
}

interface PlantComponentResponse {
  success: number;
  pC_ID?: number;
  pC_DTO?: PlantComponent;
  list_PC_DTO?: PlantComponent[];
  message?: string;
}

interface PC_DTO {
  name: string;
  description: string;
  createdByUserId?: number;
}

interface PC_DTO_Update {
  componentId: number;
  name: string;
  description: string;
  createdByUserId?: number;
}

interface PC_DTO_Delete {
  componentId: number;
}

@Component({
  selector: 'app-plantcomponent',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './plantcomponent.component.html',
  styleUrls: ['./plantcomponent.component.css']
})
export class PlantcomponentComponent implements OnInit {
  // --- Configurazione API ---
  private readonly apiUrl = 'https://localhost:7121/plantComponent';

  // --- Dati utente / autenticazione ---
  userName: string = '';
  token: string = '';
  userRole: string = '';

  // --- Stato UI ---
  loading: boolean = false;
  activeForm: string = '';

  // --- Dati per le liste e paginazione ---
  plantComponents: PlantComponent[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  // --- Dettagli singolo elemento (search) ---
  searchId: number | null = null;
  selectedPlantComponent: PlantComponent | null = null;

  // --- Dati per i form (create / update / delete) ---
  createFormData = {
    name: '',
    description: ''
  };

  updateFormData = {
    componentId: null as number | null,
    name: '',
    description: ''
  };

  deleteFormData = {
    componentId: null as number | null
  };

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.loadUserData();
    if (!this.token) {
      console.warn('Token di autenticazione non trovato → reindirizzamento al login');
      this.router.navigate(['/login']);
    }
  }

  // ============================
  // GESTIONE UTENTE / AUTENTICAZIONE
  // ============================
  private loadUserData(): void {
    if (typeof localStorage !== 'undefined') {
      this.token = localStorage.getItem('authToken') || '';
      this.userName = localStorage.getItem('userName') || 'Utente';
      this.userRole = localStorage.getItem('userRole') || '';

      console.log('Token caricato:', this.token ? 'PRESENTE' : 'MANCANTE');
      console.log('Username:', this.userName);
    }
  }

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

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  logout(): void {
    if (typeof localStorage !== 'undefined') {
      localStorage.removeItem('authToken');
      localStorage.removeItem('userName');
      localStorage.removeItem('userRole');
    }
    this.router.navigate(['/login']);
  }

  // ============================
  // GESTIONE UI E FORM
  // ============================
  showForm(formType: string): void {
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.activeForm = formType;
    // this.resetForms();

    if (formType === 'list') {
      this.loadPlantComponents();
    }
  }

  hideForm(): void {
    this.activeForm = '';
    this.resetForms();
  }

  private resetForms(): void {
    this.createFormData = { name: '', description: '' };
    this.updateFormData = { componentId: null, name: '', description: '' };
    this.deleteFormData = { componentId: null };
    this.searchId = null;
    this.selectedPlantComponent = null;
  }

  // ============================
  // CHIAMATE API
  // ============================
  loadPlantComponents(): void {
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }
    debugger;
    this.loading = true;
    const url = `${this.apiUrl}/getall/${this.currentPage}/${this.pageSize}`;

    console.log('Chiamata API →', url);
    console.log('Headers →', this.getHttpHeaders());

    this.http.get<PlantComponentResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        console.log('Risposta API →', response);

        if (response.success === 200 && response.list_PC_DTO) {
          // Il JSON restituisce array di oggetti con proprietà `componentId`
          this.plantComponents = response.list_PC_DTO;
          debugger
          this.totalCount = this.plantComponents.length;
          console.log('Plant components caricati:', this.plantComponents);
        } else {
          this.handleError('Errore nel caricamento dei plant components: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  getSinglePlantComponent(): void {
    if (!this.searchId || !this.isAuthenticated()) {
      if (!this.isAuthenticated()) this.handleAuthError();
      return;
    }

    this.loading = true;
    const url = `${this.apiUrl}/getSingle/${this.searchId}`;

    this.http.get<PlantComponentResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200 && response.pC_DTO) {
          this.selectedPlantComponent = response.pC_DTO;
          console.log('Plant component trovato:', this.selectedPlantComponent);
        } else if (response.success === 404) {
          this.selectedPlantComponent = null;
          alert('Plant component non trovato');
        } else {
          this.handleError('Errore nella ricerca: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  createPlantComponent(): void {
    debugger
    if (!this.createFormData.name.trim() || !this.createFormData.description.trim()) {
      alert('Nome e descrizione sono obbligatori');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    const dto: PC_DTO = {
      name: this.createFormData.name.trim(),
      description: this.createFormData.description.trim()
    };
    const url = `${this.apiUrl}/create`;

    this.http.post<PlantComponentResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Plant component creato con successo!');
          this.resetForms();
          this.hideForm();
        } else {
          this.handleError('Errore nella creazione: ' + (response.message || 'unknown error'));
        }
        
          this.loadPlantComponents();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  updatePlantComponent(): void {
    if (!this.updateFormData.componentId || !this.updateFormData.name.trim() || !this.updateFormData.description.trim()) {
      alert('ID, nome e descrizione sono obbligatori');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    
    const dto: PC_DTO_Update = {
      componentId: this.updateFormData.componentId,
      name: this.updateFormData.name.trim(),
      description: this.updateFormData.description.trim()
    };
    const url = `${this.apiUrl}/update`;

    this.http.post<PlantComponentResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Plant component aggiornato con successo!');
          this.resetForms();
          this.hideForm();
          if (this.plantComponents.length > 0) {
            this.loadPlantComponents();
          }
        } else {
          this.handleError('Errore nell’aggiornamento: ' + (response.message || 'unknown error'));
        }
        this.loadPlantComponents();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  deletePlantComponent(): void {
    if (!this.deleteFormData.componentId) {
      alert('ID è obbligatorio per l’eliminazione');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }
    if (!confirm('Sei sicuro di voler eliminare questo Plant Component? Questa operazione è irreversibile.')) {
      return;
    }

    this.loading = true;
    const dto: PC_DTO_Delete = { componentId: this.deleteFormData.componentId };
    const url = `${this.apiUrl}/delete`;

    this.http.delete<PlantComponentResponse>(url, {
      headers: this.getHttpHeaders(),
      body: dto
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Plant component eliminato con successo!');
          this.resetForms();
          this.hideForm();
          if (this.plantComponents.length > 0) {
            this.loadPlantComponents();
          }
        } else {
          this.handleError('Errore nell’eliminazione: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  // ============================
  // PAGINAZIONE
  // ============================
  getTotalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.getTotalPages()) {
      this.currentPage = page;
      this.loadPlantComponents();
    }
  }

  // ============================
  // UTILITY / ERROR HANDLING
  // ============================
  fillUpdateForm(pc: PlantComponent): void {
    debugger
    this.updateFormData = {
      componentId: pc.componentId,
      name: pc.name,
      description: pc.description
    };
    this.showForm('update');
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
}
