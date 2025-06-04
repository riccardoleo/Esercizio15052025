// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-tool',
//   imports: [],
//   templateUrl: './tool.component.html',
//   styleUrl: './tool.component.css'
// })
// export class ToolComponent {

// }

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

// Interfacce allineate ai DTO C#
interface Tool {
  toolId: number;
  name: string;
  description: string;
  creationDate: Date;
  categoryId: number;
  plantComponentId: number;
  createdByUserId?: number;
}

interface ToolResponse {
  success: number;
  userId?: number;
  tool_DTO?: Tool;        // Allineato al DTO C# ToolDTO_Response
  tools?: Tool[];         // Allineato al DTO C# ToolDTO_Response
  message?: string;
}

interface T_DTO {
  name: string;
  description: string;
  creationDate: Date;
  categoryId: number;
  plantComponentId: number;
  createdByUserId?: number;
}

interface T_DTO_Update {
  toolId: number;
  name: string;
  description: string;
  creationDate: Date;
  createdByUserId?: number;
}

interface T_DTO_Delete {
  toolId: number;
}

@Component({
  selector: 'app-tool',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tool.component.html',
  styleUrls: ['./tool.component.css']
})
export class ToolComponent implements OnInit {
  // --- Configurazione API ---
  private readonly apiUrl = 'https://localhost:7121/tool';

  // --- Dati utente / autenticazione ---
  userName: string = '';
  token: string = '';
  userRole: string = '';

  // --- Stato UI ---
  loading: boolean = false;
  activeForm: string = '';

  // --- Dati per le liste e paginazione ---
  tools: Tool[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  // --- Dettagli singolo elemento (search) ---
  searchId: number | null = null;
  selectedTool: Tool | null = null;

  // --- Dati per i form (create / update / delete) ---
  createFormData = {
    name: '',
    description: '',
    categoryId: 1,
    plantComponentId: 1
  };

  updateFormData = {
    toolId: null as number | null,
    name: '',
    description: '',
    creationDate: new Date()
  };

  deleteFormData = {
    toolId: null as number | null
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

    if (formType === 'list') {
      this.loadTools();
    }
  }

  hideForm(): void {
    this.activeForm = '';
    this.resetForms();
  }

  private resetForms(): void {
    this.createFormData = { 
      name: '', 
      description: '', 
      categoryId: 1, 
      plantComponentId: 1 
    };
    this.updateFormData = { 
      toolId: null, 
      name: '', 
      description: '', 
      creationDate: new Date() 
    };
    this.deleteFormData = { toolId: null };
    this.searchId = null;
    this.selectedTool = null;
  }

  // ============================
  // CHIAMATE API
  // ============================
  loadTools(): void {
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    const url = `${this.apiUrl}/getall/${this.currentPage}/${this.pageSize}`;

    console.log('Chiamata API →', url);
    console.log('Headers →', this.getHttpHeaders());

    this.http.get<ToolResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        console.log('Risposta API →', response);

        if (response.success === 200 && response.tools) {
          this.tools = response.tools;
          this.totalCount = this.tools.length;
          console.log('Tools caricati:', this.tools);
        } else {
          this.handleError('Errore nel caricamento dei tools: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  getSingleTool(): void {
    if (!this.searchId || !this.isAuthenticated()) {
      if (!this.isAuthenticated()) this.handleAuthError();
      return;
    }

    this.loading = true;
    const url = `${this.apiUrl}/getsingle/${this.searchId}`;

    this.http.get<ToolResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200 && response.tool_DTO) {
          this.selectedTool = response.tool_DTO;
          console.log('Tool trovato:', this.selectedTool);
        } else if (response.success === 404) {
          this.selectedTool = null;
          alert('Tool non trovato');
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

  createTool(): void {
    if (!this.createFormData.name.trim() || 
        !this.createFormData.description.trim() ||
        !this.createFormData.categoryId ||
        !this.createFormData.plantComponentId) {
      alert('Nome, descrizione, categoria e componente impianto sono obbligatori');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    const dto: T_DTO = {
      name: this.createFormData.name.trim(),
      description: this.createFormData.description.trim(),
      creationDate: new Date(),
      categoryId: this.createFormData.categoryId,
      plantComponentId: this.createFormData.plantComponentId
    };
    const url = `${this.apiUrl}/create`;

    this.http.post<ToolResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Tool creato con successo!');
          this.resetForms();
          this.hideForm();
        } else {
          this.handleError('Errore nella creazione: ' + (response.message || 'unknown error'));
        }
        
        this.loadTools();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  updateTool(): void {
    if (!this.updateFormData.toolId || 
        !this.updateFormData.name.trim() || 
        !this.updateFormData.description.trim()) {
      alert('ID, nome e descrizione sono obbligatori');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    
    const dto: T_DTO_Update = {
      toolId: this.updateFormData.toolId,
      name: this.updateFormData.name.trim(),
      description: this.updateFormData.description.trim(),
      creationDate: this.updateFormData.creationDate
    };
    const url = `${this.apiUrl}/update`;

    this.http.post<ToolResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Tool aggiornato con successo!');
          this.resetForms();
          this.hideForm();
          if (this.tools.length > 0) {
            this.loadTools();
          }
        } else {
          this.handleError("Errore nell'aggiornamento: " + (response.message || 'unknown error'));
        }
        this.loadTools();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  deleteTool(): void {
    if (!this.deleteFormData.toolId) {
      alert("ID è obbligatorio per l'eliminazione");
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }
    if (!confirm('Sei sicuro di voler eliminare questo Tool? Questa operazione è irreversibile.')) {
      return;
    }

    this.loading = true;
    const dto: T_DTO_Delete = { toolId: this.deleteFormData.toolId };
    const url = `${this.apiUrl}/delete`;

    this.http.delete<ToolResponse>(url, {
      headers: this.getHttpHeaders(),
      body: dto
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Tool eliminato con successo!');
          this.resetForms();
          this.hideForm();
          if (this.tools.length > 0) {
            this.loadTools();
          }
        } else {
          this.handleError("Errore nell'eliminazione: " + (response.message || 'unknown error'));
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
      this.loadTools();
    }
  }

  // ============================
  // UTILITY / ERROR HANDLING
  // ============================
  fillUpdateForm(tool: Tool): void {
    this.updateFormData = {
      toolId: tool.toolId,
      name: tool.name,
      description: tool.description,
      creationDate: tool.creationDate
    };
    this.showForm('update');
  }

  // Utility per formattare le date
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
}