## 📌 Descrizione Generale

Il servizio `Tool_Service` gestisce le operazioni CRUD sui "tool", incluse:

- Recupero di liste complete o filtrate per utente
- Recupero di un singolo tool
- Aggiunta
- Aggiornamento
- Eliminazione

Ogni metodo restituisce un oggetto `ToolDTO_Response` con:

- **success**: codice di esito
- **message**: messaggio descrittivo
- **tools** o **tool_DTO**: dati dei tool (se previsti)

## 🧾 Codici di Errore

I codici di errore seguono la notazione `[TSxxAy]`, dove:

- `TS` = "Tool Service"
- `xx` = numero del servizio
- `Ay` = codice specifico dell'errore

### Elenco Codici

| Codice | Descrizione |
| --- | --- |
| A1 | Il valore `0` non è valido per l'operazione |
| A2 | Nessun elemento trovato |
| A3 | Il valore `null` o vuoto non è valido |
| A5 | Nome già esistente nel database |
| A7 | Tool non associato all'utente specificato |

## 🛠️ Servizi Disponibili

### 1. `GetAllAsync(int index, int block)`

**Descrizione**

Recupera una lista paginata di tutti i tool disponibili nel sistema.

**Parametri**

- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[TS01A1]` se `index == 0` o `block == 0`
- `[TS01A2]` se non ci sono tool nel database

### 2. `GetAllToolsByUserAsync(int userID, int index, int block)`

**Descrizione**

Recupera i tool associati a un utente specifico, con paginazione.

**Parametri**

- `userID` (int): identificativo utente
- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[TS02A1]` se `index == 0` o `block == 0`
- `[TS02A3]` se l'ID utente non è valido o non ha tool associati

### 3. `GetByIdAsync(int id, int userID)`

**Descrizione**

Recupera un singolo tool associato a un utente specifico.

**Parametri**

- `id` (int): identificativo del tool
- `userID` (int): identificativo utente

**Errori**

- `[TS03A1]` se `id == 0`
- `[TS03A2]` se il tool non esiste nel database
- `[TS03A7]` se il tool non è associato all'utente specificato

### 4. `AddAsync(T_DTO dto)`

**Descrizione**

Aggiunge un nuovo tool al sistema.

**Parametri**

- `dto` (T_DTO): dati del tool, inclusi `Name` e `CreationDate` (opzionale)

**Logica Speciale**

- Se `CreationDate` non è specificata, viene impostata automaticamente a `DateTime.Now`

**Errori**

- `[TS04A3]` se `dto.Name` è `null` o vuoto
- `[TS04A5]` se esiste già un tool con lo stesso nome

### 5. `UpdateAsync(T_DTO_Update dto)`

**Descrizione**

Aggiorna i dati di un tool esistente.

**Parametri**

- `dto` (T_DTO_Update): dati aggiornati, inclusi `ToolId` e `Name`

**Errori**

- `[TS05A1]` se `dto.ToolId == 0`
- `[TS05A3]` se `dto.Name` è `null` o vuoto
- `[TS05A5]` se il nuovo nome è già utilizzato da un altro tool

### 6. `DeleteAsync(T_DTO_Delete dto)`

**Descrizione**

Elimina un tool dal sistema.

**Parametri**

- `dto` (T_DTO_Delete): contiene `ToolId`

**Errori**

- `[TS06A1]` se `dto.ToolId == 0`

## 🧪 Esempi di Risposta

### ✅ Successo

```json
{
  "success": 200,
  "message": "🔥 Tool trovato con successo",
  "tool_DTO": {
    "Name": "Martello Pneumatico",
    "Description" : " [descrizione Tool] "
    "CreationDate": "2025-05-30T10:30:00"
  }
}

```

### ❌ Errore

```json
{
  "success": 0,
  "message": "[TS03A1] 🚠🥀 ID inserito non valido"
}

```

### 📋 Lista Tools

```json
{
  "success": 200,
  "message": "🔥 lista tools ottenuta con successo",
  "tools": [
    {
      "Name": "Trapano",
	    "Description" : " [descrizione Tool] ",
      "CreationDate": "2025-05-29T14:20:00"
    },
    {
      "Name": "Saldatrice",
	    "Description" : " [descrizione Tool] ",
      "CreationDate": "2025-05-30T09:15:00"
    }
  ]
}

```

## 🧰 Dipendenze

- **AutoMapper**: Utilizzato per la mappatura bidirezionale tra entità e DTO (`Tool ↔ T_DTO`)
- **NLog**: Framework di logging utilizzato per tracciare warning, errori e operazioni di debug
- **ITool_Repo**: Interfaccia del repository per l'accesso ai dati relativi ai Tool
- **Microsoft.IdentityModel.Tokens**: Utilizzato per validazioni sui token (metodo `IsNullOrEmpty`)

## 🔒 Controlli di Sicurezza

Il servizio implementa controlli di ownership per garantire che:

- Gli utenti possano accedere solo ai propri tool
- Le operazioni di lettura, modifica ed eliminazione siano limitate ai tool di proprietà dell'utente
- La validazione dei nomi eviti duplicati nel sistema

## 📝 Note Tecniche

- Tutti i messaggi di successo utilizzano l'emoji 🔥
- I messaggi di errore utilizzano le emoji 🚠🥀 o 💔
- Il logging viene effettuato per tutti gli errori e warning
- La paginazione è obbligatoria per le operazioni di lettura di liste