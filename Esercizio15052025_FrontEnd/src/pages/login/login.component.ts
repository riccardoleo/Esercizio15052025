import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Interfaccia per la risposta del backend
interface UserResponseDTO {
  success: number;
  UserId?: number;
  user_DTO?: any;
  users?: any[];
  token?: string;
  message?: string;
  userRole?: string;
  IsAdmin?: boolean;
}

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  @ViewChild('usernameInput') usernameInput!: ElementRef;
  @ViewChild('passwordInput') passwordInput!: ElementRef;

  private http = inject(HttpClient);
  private router = inject(Router);

  onSubmit() {
    debugger;
    const username = this.usernameInput.nativeElement.value;
    const password = this.passwordInput.nativeElement.value;

    console.log('Username:', username);
    console.log('Password:', password);

    const loginData = {
      username: username,
      passwordHash: password
    };

    this.http.post<UserResponseDTO>("https://localhost:7121/user/login", loginData, {
      headers: {
        'accept': '*/*',
        'Content-Type': 'application/json'
      }
    }).subscribe({
      next: (response: UserResponseDTO) => {
        console.log('Login successful:', response);
        console.log('Success code:', response.success);
        console.log('Token:', response.token);
        console.log('User Role:', response.userRole);

        debugger;

        if (response.token && response.success === 200) {
          console.log('Token ricevuto:', response.token);
  
          // Salva il token e il ruolo
          localStorage.setItem('authToken', response.token);
          if (response.userRole) {
            localStorage.setItem('userRole', response.userRole);
          }
  
          // AGGIUNGI QUESTO: Salva il nome utente
          if (response.user_DTO?.name) {
            localStorage.setItem('userName', response.user_DTO.name);
          } else if (response.user_DTO?.username) {
            localStorage.setItem('userName', response.user_DTO.username);
          } else {
            // Se non c'è nel user_DTO, usa lo username inserito nel form
            localStorage.setItem('userName', username);
          }
  
          console.log('Dati salvati in localStorage:', {
           token: response.token,
           userRole: response.userRole,
           userName: localStorage.getItem('userName')
          });
  
          debugger;
          // Naviga al dashboard
          this.router.navigate(['/dashboard']);
        } else {
          console.error(response.message);
        }
      },
      error: (error) => {
        console.error('Login failed:', error);
        console.error('Error status:', error.status);
        console.error('Error message:', error.message);
      }
    });
  }
}