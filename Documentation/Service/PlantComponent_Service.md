## 📌 Descrizione Generale

Il servizio `PlantComponent_Service` gestisce le operazioni CRUD sui "plant component", incluse:

- Recupero di liste complete o filtrate per utente
- Recupero di un singolo plant component
- Aggiunta
- Aggiornamento
- Eliminazione

Ogni metodo restituisce un oggetto `PlantComponent_Response` con:

- **success**: codice di esito
- **message**: messaggio descrittivo
- **List_PC_DTO** o **PC_DTO**: dati dei plant component (se previsti)

## 🧾 Codici di Errore

I codici di errore seguono la notazione `[PCxxAy]`, dove:

- `PC` = "Plant Component"
- `xx` = numero del servizio
- `Ay` = codice specifico dell'errore

### Elenco Codici

| Codice | Descrizione |
| --- | --- |
| A1 | Il valore `0` non è valido per l'operazione |
| A2 | Nessun elemento trovato |
| A3 | Il valore `null` o vuoto non è valido |

## 🛠️ Servizi Disponibili

### 1. `GetAllAsync(int index, int block)`

**Descrizione**

Recupera una lista paginata di tutti i plant component disponibili nel sistema.

**Parametri**

- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[PC01A1]` se `index == 0` o `block == 0`
- `[PC01A2]` se non ci sono plant component nel database

### 2. `GetAllPlantComponentsByUserAsync(int userID, int index, int block)`

**Descrizione**

Recupera i plant component associati a un utente specifico, con paginazione.

**Parametri**

- `userID` (int): identificativo utente
- `index` (int): numero di pagina (> 0)
- `block` (int): elementi per pagina (> 0)

**Errori**

- `[PC02A3]` se l'ID utente non è valido o non ha plant component associati

### 3. `GetByIdAsync(int id, int userID)`

**Descrizione**

Recupera un singolo plant component associato a un utente specifico.

**Parametri**

- `id` (int): identificativo del plant component
- `userID` (int): identificativo utente

**Errori**

- `[PC03A1]` se `id == 0`
- `[PC03A2]` se il plant component non esiste nel database o non è associato all'utente specificato

### 4. `AddAsync(PC_DTO dto)`

**Descrizione**

Aggiunge un nuovo plant component al sistema.

**Parametri**

- `dto` (PC_DTO): dati del plant component, inclusi `Name`

**Errori**

- `[PC04A3]` se `dto.Name` è `null` o vuoto

### 5. `UpdateAsync(PC_DTO_Update dto)`

**Descrizione**

Aggiorna i dati di un plant component esistente.

**Parametri**

- `dto` (PC_DTO_Update): dati aggiornati, inclusi `ComponentId` e `Name`

**Errori**

- `[PC05A1]` se `dto.ComponentId == 0`
- `[PC05A3]` se `dto.Name` è `null` o vuoto

### 6. `DeleteAsync(PC_DTO_Delete dto)`

**Descrizione**

Elimina un plant component dal sistema.

**Parametri**

- `dto` (PC_DTO_Delete): contiene `ComponentId`

**Errori**

- `[PC06A1]` se `dto.ComponentId == 0`

## 🧪 Esempi di Risposta

### ✅ Successo

```json
{
  "success": 200,
  "message": "🔥 Plant Component trovato con successo",
  "PC_DTO": {
    "Name": "Filtro Olio",
    "Description" : " [descrizione del filtro dell'olio] "
  }
}

```

### ❌ Errore

```json
{
  "success": 0,
  "message": "[PC03A1] 🚠🥀 ID inserito non valido"
}

```

### 📋 Lista Plant Components

```json
{
  "success": 200,
  "message": "🔥 lista PlantComponent ottenuta con successo",
  "List_PC_DTO": [
    {
      "Name": "Filtro Olio",
	    "Description" : " [descrizione del filtro dell'olio] "
    },
    {
      "Name": "Pompa Idraulica",
      "Description" : " [descrizione della poma idraulica] "
    }
  ]
}

```

## 🧰 Dipendenze

- **AutoMapper**: Utilizzato per la mappatura bidirezionale tra entità e DTO (`PlantComponent ↔ PC_DTO`)
- **NLog**: Framework di logging utilizzato per tracciare warning, errori e operazioni di debug
- **IPlantComponent_Repo**: Interfaccia del repository per l'accesso ai dati relativi ai Plant Component
- **Microsoft.IdentityModel.Tokens**: Utilizzato per validazioni sui token (metodo `IsNullOrEmpty`)

## 🔒 Controlli di Sicurezza

Il servizio implementa controlli di ownership per garantire che:

- Gli utenti possano accedere solo ai propri plant component
- Le operazioni di lettura, modifica ed eliminazione siano limitate ai plant component di proprietà dell'utente
- La validazione dei dati eviti inserimenti di valori nulli o vuoti

## 📝 Note Tecniche

- Tutti i messaggi di successo utilizzano l'emoji 🔥
- I messaggi di errore utilizzano le emoji 🚠🥀 o 💔
- Il logging viene effettuato per tutti gli errori e warning
- La paginazione è obbligatoria per le operazioni di lettura di liste
- A differenza del Tool_Service, non è presente il controllo per nomi duplicati nel PlantComponent_Service