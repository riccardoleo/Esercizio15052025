## 📌 Descrizione Generale

Il servizio `ToolCategory_Service` gestisce le operazioni CRUD sulle "tool category", incluse:

- Recupero di liste complete o filtrate per utente
- Recupero di una singola tool category
- Aggiunta
- Aggiornamento
- Eliminazione

Ogni metodo restituisce un oggetto `ToolCategoryDTO_Response` con:

- **success**: codice di esito
- **message**: messaggio descrittivo
- **toolCategories** o **toolCategory_DTO**: dati delle tool category (se previsti)

## 🧾 Codici di Errore

I codici di errore seguono la notazione `[TCxxAy]`, dove:

- `TC` = "Tool Category"
- `xx` = numero del servizio
- `Ay` = codice specifico dell'errore

### Elenco Codici

| Codice | Descrizione |
| --- | --- |
| A1 | Il valore `0` non è valido per l'operazione |
| A2 | Nessun elemento trovato |
| A3 | Il valore `null` o vuoto non è valido |
| A7 | La tool category non è associata all'utente specificato |

## 🛠️ Servizi Disponibili

### 1. `GetAllAsync(int index, int block)`

**Descrizione**

Recupera una lista paginata di tutte le tool category disponibili nel sistema.

**Parametri**

- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[TC01A1]` se `index == 0` o `block == 0`
- `[TC01A2]` se non ci sono tool category nel database

### 2. `GetAllToolCategoriesByUserAsync(int userID, int index, int block)`

**Descrizione**

Recupera le tool category associate a un utente specifico, con paginazione.

**Parametri**

- `userID` (int): identificativo utente
- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[TC02A1]` se `index == 0` o `block == 0`
- `[TC02A2]` se non ci sono tool category associate all'utente

### 3. `GetByIdAsync(int id, int userID)`

**Descrizione**

Recupera una singola tool category associata a un utente specifico.

**Parametri**

- `id` (int): identificativo della tool category
- `userID` (int): identificativo utente

**Errori**

- `[TC03A1]` se `id == 0`
- `[TC03A2]` se la tool category non esiste nel database
- `[TC03A7]` se la tool category non è associata all'utente specificato

### 4. `AddAsync(TC_DTO dto)`

**Descrizione**

Aggiunge una nuova tool category al sistema.

**Parametri**

- `dto` (TC_DTO): dati della tool category, inclusi `Name` e opzionalmente `CreatedByUserId`

**Errori**

- `[TC04A3]` se `dto.Name` è `null` o vuoto

### 5. `UpdateAsync(TC_DTO_Update dto)`

**Descrizione**

Aggiorna i dati di una tool category esistente.

**Parametri**

- `dto` (TC_DTO_Update): dati aggiornati, inclusi `CategoryId` e `Name`

**Errori**

- `[TC05A1]` se `dto.CategoryId == 0`
- `[TC05A3]` se `dto.Name` è `null` o vuoto

### 6. `DeleteAsync(TC_DTO_Delete dto)`

**Descrizione**

Elimina una tool category dal sistema.

**Parametri**

- `dto` (TC_DTO_Delete): contiene `CategoryId`

**Errori**

- `[TC06A1]` se `dto.CategoryId == 0`

## 🧪 Esempi di Risposta

### ✅ Successo

```json
{
  "success": 200,
  "message": "🔥 ToolCategory trovato con successo",
  "toolCategory_DTO": {
    "Name": "Utensili da Taglio",
    "CreatedByUserId": 123
  }
}

```

### ❌ Errore

```json
{
  "success": 0,
  "message": "[TC03A1] 🚠🥀 ID inserito non valido"
}

```

### 📋 Lista Tool Categories

```json
{
  "success": 200,
  "message": "🔥 lista toolCategories ottenuta con successo",
  "toolCategories": [
    {
      "Name": "Utensili da Taglio",
      "CreatedByUserId": 123
    },
    {
      "Name": "Strumenti di Misura",
      "CreatedByUserId": 456
    }
  ]
}

```

## 🧰 Dipendenze

- **AutoMapper**: Utilizzato per la mappatura bidirezionale tra entità e DTO (`ToolCategory ↔ TC_DTO`)
- **NLog**: Framework di logging utilizzato per tracciare warning, errori e operazioni di debug
- **IToolCategory_Repo**: Interfaccia del repository per l'accesso ai dati relativi alle Tool Category
- **Microsoft.IdentityModel. Tokens**: Utilizzato per validazioni sui token (metodo `IsNullOrEmpty`)

## 🔒 Controlli di Sicurezza

Il servizio implementa controlli di ownership per garantire che:

- Gli utenti possano accedere solo alle proprie tool category
- Le operazioni di lettura, modifica ed eliminazione siano limitate alle tool category di proprietà dell'utente
- La validazione dei dati eviti inserimenti di valori nulli o vuoti

## 📝 Note Tecniche

- Tutti i messaggi di successo utilizzano l'emoji 🔥
- I messaggi di errore utilizzano le emoji 🚠🥀 o 💔
- Il logging viene effettuato per tutti gli errori e warning
- La paginazione è obbligatoria per le operazioni di lettura di liste
- Il controllo di ownership viene effettuato tramite il metodo `IsToolCategoryOwnedByUserAsync`
- Il campo `CreatedByUserId` è opzionale nel DTO di creazione