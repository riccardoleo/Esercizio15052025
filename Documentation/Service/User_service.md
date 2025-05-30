## 📌 Descrizione Generale

Il servizio `User_Service` gestisce le operazioni CRUD sugli utenti e l'autenticazione, incluse:

- Recupero di liste complete di utenti
- Recupero di un singolo utente
- Login con autenticazione JWT
- Registrazione
- Aggiornamento
- Eliminazione
- Utilità per recupero ID e controllo ruoli

Ogni metodo restituisce un oggetto `UserResponseDTO` con:

- **success**: codice di esito
- **message**: messaggio descrittivo
- **users**, **user_DTO**, **token**, **UserId**, **UserRole**: dati specifici (se previsti)

## 🧾 Codici di Errore

I codici di errore seguono la notazione `[USxxAy]`, dove:

- `US` = "User Service"
- `xx` = numero del servizio
- `Ay` = codice specifico dell'errore

### Elenco Codici

| Codice | Descrizione |
| --- | --- |
| A1 | Il valore `0` non è valido per l'operazione |
| A2 | Utente non trovato |
| A3 | Dati utente non validi o null |
| A4 | Password errata |
| A5 | Nome utente già esistente |
| A6 | Ruolo utente non valido |

## 🛠️ Servizi Disponibili

### 1. `GetAllAsync(int index, int block)`

**Descrizione**

Recupera una lista paginata di tutti gli utenti disponibili nel sistema.

**Parametri**

- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Logica Speciale**

- Se esiste un solo utente nel sistema, restituisce il messaggio "💔 sei solo amico"

**Errori**

- `[US01A1]` se `index == 0` o `block == 0`
- `[US01A2]` se non ci sono utenti nel database

### 2. `GetByIdAsync(int id)`

**Descrizione**

Recupera un singolo utente tramite il suo ID.

**Parametri**

- `id` (int): identificativo dell'utente

**Errori**

- `[US02A3]` se l'ID non è valido o l'utente non esiste

### 3. `Login(UserCredential_DTO dto)`

**Descrizione**

Autentica un utente e restituisce un token JWT valido per 5 minuti.

**Parametri**

- `dto` (UserCredential_DTO): contiene `Username` e `PasswordHash`

**Funzionalità JWT**

- Issuer/Audience: `https://localhost:7121`
- Algoritmo: HmacSha256
- Scadenza: 5 minuti
- Claims: Username (Sub) e Role

**Errori**

- `[US03A3]` se il DTO è null
- `[US03A2]` se l'utente non esiste
- `[US03A4]` se la password non corrisponde

### 4. `RegisterAsync(User_DTO dto)`

**Descrizione**

Registra un nuovo utente nel sistema con hash della password.

**Parametri**

- `dto` (User_DTO): dati completi dell'utente

**Logica Speciale**

- Se `passwdRole == "adminPassword"`, l'utente diventa admin
- Altrimenti viene assegnato il ruolo "user"
- La password viene automaticamente hashata con BCrypt

**Errori**

- `[US04A3]` se il DTO è null o Username è vuoto
- `[US04A5]` se l'username è già esistente

### 5. `UpdateAsync(User_DTO dto)`

**Descrizione**

Aggiorna i dati di un utente esistente previa verifica della password.

**Parametri**

- `dto` (User_DTO): dati aggiornati dell'utente

**Sicurezza**

- Richiede verifica della password corrente
- La password nella risposta viene mascherata come "##############"

**Errori**

- `[US05A3]` se Username è vuoto
- `[US05A4]` se la password non corrisponde

### 6. `DeleteAsync(UserCredential_DTO dto)`

**Descrizione**

Elimina un utente dal sistema previa verifica delle credenziali.

**Parametri**

- `dto` (UserCredential_DTO): credenziali per la verifica

**Errori**

- `[US06A3]` se Username è null
- `[US06A4]` se la password non corrisponde

### 7. `UserIDFromUserName(string username)`

**Descrizione**

Restituisce l'ID di un utente dato il suo username.

**Parametri**

- `username` (string): nome utente

**Errori**

- `[US07A4]` se username è null
- `[US07A2]` se l'utente non esiste

### 8. `CheckRole(string userRole)`

**Descrizione**

Valida e normalizza il ruolo di un utente.

**Parametri**

- `userRole` (string): ruolo da verificare ("admin" o "user")

**Logica**

- Converte automaticamente il ruolo in lowercase
- Accetta solo "admin" o "user"

**Errori**

- `[US08A3]` se userRole è null
- `[US08A6]` se il ruolo non è "admin" o "user"

## 🧪 Esempi di Risposta

### ✅ Login Riuscito

```json
{
  "success": 200,
  "message": "🔥 Utente loggato con successo",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

```

### ✅ Utente Trovato

```json
{
  "success": 200,
  "message": "🔥 Utente trovato con successo",
  "user_DTO": {
    "Username": "mario.rossi",
    "Email": "mario@example.com",
    "Role": "user",
    "CreatedAt": "2025-05-30T10:30:00"
  }
}

```

### ❌ Errore di Autenticazione

```json
{
  "success": 0,
  "message": "[US03A4] 🚠🥀 Dati utente non validi"
}

```

### 📋 Lista Utenti

```json
{
  "success": 200,
  "message": "🔥 lista utenti ottenuta con successo",
  "users": [
    {
      "Username": "admin",
      "Email": "admin@example.com",
      "Role": "admin"
    },
    {
      "Username": "user1",
      "Email": "user1@example.com",
      "Role": "user"
    }
  ]
}

```

## 🧰 Dipendenze

- **AutoMapper**: Utilizzato per la mappatura bidirezionale tra entità e DTO (`User ↔ User_DTO`)
- **NLog**: Framework di logging utilizzato per tracciare warning, errori e operazioni di debug
- **IUser_Repo**: Interfaccia del repository per l'accesso ai dati relativi agli User
- **BCrypt.Net**: Libreria per l'hashing sicuro delle password
- **System.IdentityModel.Tokens.Jwt**: Libreria per la gestione dei token JWT
- **Microsoft.IdentityModel.Tokens**: Utilizzato per validazioni sui token

## 🔒 Controlli di Sicurezza

Il servizio implementa diversi livelli di sicurezza:

- **Hash delle password** con BCrypt per proteggere le credenziali
- **Autenticazione JWT** con scadenza limitata (5 minuti)
- **Verifica password** per operazioni sensibili (update/delete)
- **Controllo duplicati** per prevenire username duplicati
- **Gestione ruoli** con validazione rigorosa
- **Mascheramento password** nelle risposte API

## 📝 Note Tecniche

- Tutti i messaggi di successo utilizzano l'emoji 🔥
- I messaggi di errore utilizzano le emoji 🚠🥀 o 💔
- Il logging viene effettuato per tutti gli errori e warning
- La paginazione è obbligatoria per `GetAllAsync`
- La chiave segreta JWT è hardcoded (da cambiare in produzione)
- Il sistema supporta due ruoli: "admin" e "user"
- Password speciale "adminPassword" per ottenere privilegi admin durante la registrazione
- Messaggio speciale "💔 sei solo amico" quando c'è un solo utente nel sistema

## ⚠️ Considerazioni per la Produzione

- La chiave JWT dovrebbe essere spostata in configurazione sicura
- La password admin speciale dovrebbe essere rimossa o resa più sicura
- Considerare l'implementazione di rate limiting per il login
- Valutare l'estensione della durata del token JWT