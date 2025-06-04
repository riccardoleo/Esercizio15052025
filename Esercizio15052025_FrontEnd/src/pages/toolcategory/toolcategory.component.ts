// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-toolcategory',
//   imports: [],
//   templateUrl: './toolcategory.component.html',
//   styleUrl: './toolcategory.component.css'
// })
// export class ToolcategoryComponent {

// }

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

// Interfacce allineate ai DTO C#
interface ToolCategory {
  categoryId: number;
  name: string;
  createdByUserId?: number;
}

interface ToolCategoryResponse {
  success: number;
  userId?: number;
  toolCategory_DTO?: ToolCategory;    // Allineato al DTO C# ToolCategoryDTO_Response
  toolCategories?: ToolCategory[];    // Allineato al DTO C# ToolCategoryDTO_Response
  message?: string;
}

interface TC_DTO {
  name: string;
  categoryId?: number;
  createdByUserId?: number;
}

interface TC_DTO_Update {
  categoryId: number;
  name: string;
  createdByUserId?: number;
}

interface TC_DTO_Delete {
  categoryId: number;
}

@Component({
  selector: 'app-toolcategory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './toolcategory.component.html',
  styleUrls: ['./toolcategory.component.css']
})
export class ToolCategoryComponent implements OnInit {
  // --- Configurazione API ---
  private readonly apiUrl = 'https://localhost:7121/toolcategory';

  // --- Dati utente / autenticazione ---
  userName: string = '';
  token: string = '';
  userRole: string = '';

  // --- Stato UI ---
  loading: boolean = false;
  activeForm: string = '';

  // --- Dati per le liste e paginazione ---
  toolCategories: ToolCategory[] = [];
  totalCount: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;

  // --- Dettagli singolo elemento (search) ---
  searchId: number | null = null;
  selectedToolCategory: ToolCategory | null = null;

  // --- Dati per i form (create / update / delete) ---
  createFormData = {
    name: ''
  };

  updateFormData = {
    categoryId: null as number | null,
    name: ''
  };

  deleteFormData = {
    categoryId: null as number | null
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
      this.loadToolCategories();
    }
  }

  hideForm(): void {
    this.activeForm = '';
    this.resetForms();
  }

  private resetForms(): void {
    this.createFormData = { name: '' };
    this.updateFormData = { 
      categoryId: null, 
      name: '' 
    };
    this.deleteFormData = { categoryId: null };
    this.searchId = null;
    this.selectedToolCategory = null;
  }

  // ============================
  // CHIAMATE API
  // ============================
  loadToolCategories(): void {
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    const url = `${this.apiUrl}/getall/${this.currentPage}/${this.pageSize}`;

    console.log('Chiamata API →', url);
    console.log('Headers →', this.getHttpHeaders());

    this.http.get<ToolCategoryResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        console.log('Risposta API →', response);

        if (response.success === 200 && response.toolCategories) {
          this.toolCategories = response.toolCategories;
          this.totalCount = this.toolCategories.length;
          console.log('Tool Categories caricate:', this.toolCategories);
        } else {
          this.handleError('Errore nel caricamento delle categorie: ' + (response.message || 'unknown error'));
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  getSingleToolCategory(): void {
    if (!this.searchId || !this.isAuthenticated()) {
      if (!this.isAuthenticated()) this.handleAuthError();
      return;
    }

    this.loading = true;
    const url = `${this.apiUrl}/getsingle/${this.searchId}`;

    this.http.get<ToolCategoryResponse>(url, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200 && response.toolCategory_DTO) {
          this.selectedToolCategory = response.toolCategory_DTO;
          console.log('Tool Category trovata:', this.selectedToolCategory);
        } else if (response.success === 404) {
          this.selectedToolCategory = null;
          alert('Categoria non trovata');
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

  createToolCategory(): void {
    if (!this.createFormData.name.trim()) {
      alert('Il nome è obbligatorio');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    const dto: TC_DTO = {
      name: this.createFormData.name.trim()
    };
    const url = `${this.apiUrl}/create`;

    this.http.post<ToolCategoryResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Categoria creata con successo!');
          this.resetForms();
          this.hideForm();
        } else {
          this.handleError('Errore nella creazione: ' + (response.message || 'unknown error'));
        }
        
        this.loadToolCategories();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  updateToolCategory(): void {
    if (!this.updateFormData.categoryId || 
        !this.updateFormData.name.trim()) {
      alert('ID e nome sono obbligatori');
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }

    this.loading = true;
    
    const dto: TC_DTO_Update = {
      categoryId: this.updateFormData.categoryId,
      name: this.updateFormData.name.trim()
    };
    const url = `${this.apiUrl}/update`;

    this.http.post<ToolCategoryResponse>(url, dto, {
      headers: this.getHttpHeaders(),
      params: { username: this.userName }
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Categoria aggiornata con successo!');
          this.resetForms();
          this.hideForm();
          if (this.toolCategories.length > 0) {
            this.loadToolCategories();
          }
        } else {
          this.handleError("Errore nell'aggiornamento: " + (response.message || 'unknown error'));
        }
        this.loadToolCategories();
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        this.handleHttpError(error);
      }
    });
  }

  deleteToolCategory(): void {
    if (!this.deleteFormData.categoryId) {
      alert("ID è obbligatorio per l'eliminazione");
      return;
    }
    if (!this.isAuthenticated()) {
      this.handleAuthError();
      return;
    }
    if (!confirm('Sei sicuro di voler eliminare questa categoria? Questa operazione è irreversibile.')) {
      return;
    }

    this.loading = true;
    const dto: TC_DTO_Delete = { categoryId: this.deleteFormData.categoryId };
    const url = `${this.apiUrl}/delete`;

    this.http.delete<ToolCategoryResponse>(url, {
      headers: this.getHttpHeaders(),
      body: dto
    }).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success === 200) {
          alert('Categoria eliminata con successo!');
          this.resetForms();
          this.hideForm();
          if (this.toolCategories.length > 0) {
            this.loadToolCategories();
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
      this.loadToolCategories();
    }
  }

  // ============================
  // UTILITY / ERROR HANDLING
  // ============================
  fillUpdateForm(toolCategory: ToolCategory): void {
    this.updateFormData = {
      categoryId: toolCategory.categoryId,
      name: toolCategory.name
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