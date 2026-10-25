# Progetto Orario scolastico — Analisi e progettazione del primo prototipo C++

**Destinatari:** studenti di Informatica, classe quarta  
**Linguaggio:** C++  
**Applicazione da realizzare:** programma Console con menu  
**Obiettivo del documento:** comprendere il problema, progettare le classi e realizzare autonomamente i file `.cpp` a partire dai file `.h` forniti.

## 1. Il problema da risolvere

Un orario scolastico indica quali lezioni si svolgono in ogni ora dei diversi giorni della settimana. A volte è necessario modificare un orario già esistente. Una modifica apparentemente semplice, come lo scambio di due lezioni, può però produrre una situazione non accettabile.

In questo progetto realizzerai un **primo prototipo** che rappresenta l'orario di una classe e consente di **scambiare il contenuto di due celle**. Il programma dovrà verificare una sola regola: **nessuna giornata può avere un'ora vuota prima di un'ora successiva occupata**.

Se lo scambio rispetta la regola, viene confermato; altrimenti il programma ripristina l'orario precedente.

Il prototipo non crea automaticamente un nuovo orario e non valuta la distribuzione delle materie. In questa prima versione non considera disponibilità dei docenti, conflitti tra aule o altri vincoli organizzativi.

## 2. Conoscenze necessarie

Per affrontare il progetto utilizzerai argomenti già studiati:

- classi, oggetti, attributi, metodi e incapsulamento;
- costruttori e distruttori;
- array, matrici e array di oggetti;
- puntatori, riferimenti e aritmetica dei puntatori;
- memoria statica, automatica e dinamica;
- passaggio dei parametri per valore, per riferimento e tramite puntatore;
- condizioni, cicli, funzioni, `new`, `nullptr` e `std::string`.

**Non sono necessari** `vector`, `stack`, `queue`, template, ereditarietà o sovraccarico degli operatori. La soluzione usa array di dimensione fissa e istruzioni C++ già note.

### Che cosa imparerai a fare

Al termine del lavoro dovrai saper distinguere il ruolo dei dati, delle regole di controllo e dell'interfaccia; gestire una matrice di puntatori; spiegare come avviene uno scambio senza copiare gli oggetti; riconoscere i principali rischi della memoria dinamica.

## 3. Requisiti del primo prototipo

L'orario contiene **5 giorni**, dal lunedì al venerdì, e **8 ore** per ogni giorno: in totale **40 celle**.

Ogni cella può essere:

- **occupata**, se contiene una tessera che descrive una lezione;
- **vuota**, se non contiene alcuna tessera.

All'avvio il programma prepara un orario di esempio con **7 lezioni dal lunedì al giovedì e 6 il venerdì** (34 lezioni complessive). Le materie e le aule sono dati dimostrativi.

Il programma deve permettere di visualizzare l'orario, consultare una cella, tentare uno scambio, controllare la validità dell'orario e uscire attraverso un menu.

### L'unico vincolo da rispettare

Le lezioni di una giornata devono cominciare dalla prima ora ed essere consecutive. Dopo la prima cella vuota, tutte le altre celle di quel giorno devono essere vuote.

| Esempio di giornata | Esito | Motivo |
|---|---|---|
| `X X X X X — — —` | Valido | Le lezioni sono consecutive |
| `X X X X X X X X` | Valido | Tutte le ore sono occupate |
| `— — — — — — — —` | Valido | Non ci sono lezioni dopo una cella vuota |
| `X X — X X — — —` | Non valido | C'è un'ora vuota prima di altre lezioni |
| `— X X X — — — —` | Non valido | La prima ora è vuota, ma seguono lezioni |

`X` rappresenta una cella occupata; `—` una cella vuota.

## 4. Architettura del programma

Il progetto è suddiviso in tre parti, ciascuna con una responsabilità precisa.

```text
                 INTERFACCIA CONSOLE
               main.cpp: menu e input
                         |
                         v
                       ENGINE
          MotoreScambi + ValidatoreOrario
                         |
                         v
                        MODEL
              Tessera + Posizione + Orario
```

**Model** — contiene le classi che rappresentano le informazioni e le operazioni elementari sulle celle. Non legge dati da tastiera e non stampa il menu.

**Engine** — applica la regola di validità e decide se confermare o annullare uno scambio. Non gestisce l'interazione con l'utente.

**Interfaccia Console** — mostra i dati, legge le scelte e richiama i metodi pubblici delle altre classi. Non accede direttamente agli attributi privati dell'orario.

Questa separazione permette di verificare una classe alla volta prima di costruire il programma completo.

## 5. Come rappresentare l'orario in memoria

La classe `Orario` contiene una matrice di **puntatori a `Tessera`**:

```cpp
Tessera* celle[5][8];
```

Questa dichiarazione **non crea 40 oggetti `Tessera`**: crea 40 puntatori. Inizialmente tutti devono contenere `nullptr`.

Quando serve una lezione, si crea un oggetto dinamico:

```cpp
Tessera* t = new Tessera("Matematica", "A1");
```

Il puntatore `t` può poi essere inserito in una cella vuota dell'orario.

```text
Oggetto Orario (per esempio variabile locale in main)

celle[0][0] -----> Tessera("Matematica", "A1") [heap]
celle[0][1] -----> Tessera("Italiano", "A2")   [heap]
celle[0][2] -----> Tessera("Inglese", "A3")   [heap]
   ...
celle[0][7] -----> nullptr
   ...
celle[4][7] -----> nullptr
```

**Attenzione:** la matrice fa parte dell'oggetto `Orario`; non è necessario crearla dinamicamente. Sono le singole tessere a essere allocate nell'heap.

### Regola di gestione delle tessere

Per semplificare il progetto adottiamo queste convenzioni:

1. Ogni tessera dinamica presente nell'orario appartiene a **una sola cella**.
2. `Orario` è responsabile della durata delle tessere che contiene.
3. `Get()` permette di consultare una tessera, ma non ne trasferisce la proprietà.
4. `Set()` inserisce un puntatore **solo in una cella vuota**.
5. `ScambiaCelle()` sposta i puntatori, senza creare o distruggere tessere.
6. Il distruttore di `Orario` deve liberare correttamente la memoria delle tessere rimaste nelle celle.
7. Nel primo prototipo **non copiare un oggetto `Orario` in un altro**: la copia automatica duplicherebbe gli indirizzi, non le tessere.

### Come deallocare le tessere alla fine del programma

Il distruttore `~Orario()` deve esaminare **tutte le 40 celle** della matrice. Non deve liberare la matrice stessa, perché `celle[5][8]` è un attributo dell'oggetto e non è stata creata con `new`.

L'algoritmo da implementare in `Orario.cpp` è il seguente:

1. Scorri i giorni con un ciclo da `0` a `4`.
2. Per ogni giorno scorri le ore con un ciclo da `0` a `7`.
3. Controlla il puntatore presente nella cella.
4. Se è diverso da `nullptr`, libera **l'oggetto dinamico puntato** tramite l'operatore `delete`.
5. Imposta il puntatore della cella a `nullptr` per rendere esplicito che non punta più a una tessera valida.

**Frammento illustrativo relativo a una sola cella** (non è l'implementazione completa del distruttore):

```cpp
if (celle[giorno][ora] != nullptr) {
    delete celle[giorno][ora];
    celle[giorno][ora] = nullptr;
}
```

Nel programma completo questo frammento deve essere eseguito all'interno dei due cicli annidati. Perché sia corretto, ogni oggetto deve appartenere a **una sola cella**: se due celle conservassero lo stesso indirizzo, il distruttore tenterebbe di liberare due volte lo stesso oggetto.

La distruzione dell'oggetto `Orario` avviene automaticamente quando la variabile locale che lo contiene termina la propria vita; è in quel momento che viene eseguito il suo distruttore. Il metodo `Set()` non deve liberare oggetti: inserisce un puntatore soltanto in una cella vuota, mentre `ScambiaCelle()` cambia la posizione dei puntatori senza modificare il numero di tessere esistenti.

**Esercizio:** scrivi in `Orario.cpp` il distruttore completo con i due cicli annidati.

## 6. Le classi del Model

Per ogni classe trovi la descrizione, il file di intestazione **completo** e le indicazioni per realizzare autonomamente il corrispondente file `.cpp`.

### 6.1 Classe `Tessera`

Una `Tessera` rappresenta una lezione. Contiene il nome della materia e l'aula in cui si svolge.

**File `Model/Tessera.h`**

```cpp
#pragma once

#include <string>

class Tessera {
private:
    std::string materia;
    std::string aula;

public:
    Tessera(std::string materia, std::string aula);

    std::string GetMateria();
    std::string GetAula();
};

```

**Da realizzare in `Tessera.cpp`:** il costruttore che inizializza gli attributi e i due metodi che restituiscono materia e aula.

**Esempio d'uso:**

```cpp
Tessera lezione("Informatica", "Lab1");
std::cout << lezione.GetMateria() << std::endl;
```

### 6.2 Classe `Posizione`

Una `Posizione` identifica una cella attraverso due numeri interi: il giorno e l'ora.

Per accedere alla matrice, gli indici partono da zero: giorni da `0` a `4`, ore da `0` a `7`. L'interfaccia potrà invece chiedere all'utente numeri da 1 a 5 e da 1 a 8.

**File `Model/Posizione.h`**

```cpp
#pragma once

class Posizione {
private:
    int giorno;
    int ora;

public:
    Posizione(int giorno, int ora);

    int GetGiorno();
    int GetOra();
};

```

**Da realizzare in `Posizione.cpp`:** il costruttore e i metodi di lettura degli indici.

**Esempio d'uso:**

```cpp
Posizione p(0, 2);  // Lunedì, terza ora
std::cout << p.GetGiorno() << " " << p.GetOra();
```

### 6.3 Classe `Orario`

`Orario` conserva la matrice e rende disponibili le operazioni di lettura, inserimento e scambio. La matrice è privata: gli altri componenti devono utilizzare i suoi metodi.

**File `Model/Orario.h`**

```cpp
#pragma once

#include "Tessera.h"
#include "Posizione.h"

class Orario {
private:
    Tessera* celle[5][8];

public:
    Orario();
    ~Orario();

    Tessera* Get(Posizione posizione);
    bool Set(Posizione posizione, Tessera* tessera);
    void ScambiaCelle(Posizione prima, Posizione seconda);
};

```

**Da realizzare in `Orario.cpp`:**

- `Orario()` — imposta tutti i puntatori della matrice a `nullptr`.
- `~Orario()` — libera la memoria delle tessere possedute dall'orario, una sola volta per ogni tessera.
- `Get()` — restituisce il puntatore presente nella cella richiesta; se la cella è vuota restituisce `nullptr`.
- `Set()` — se la cella è vuota, memorizza il puntatore ricevuto e restituisce `true`; se è occupata, non cambia nulla e restituisce `false`.
- `ScambiaCelle()` — scambia i puntatori di due celle, anche quando una delle due è vuota.

**Precondizione:** i metodi ricevono posizioni valide. Il menu deve controllare gli indici prima delle chiamate.

**Esempio di creazione e inserimento:**

```cpp
Orario orario;
Tessera* t = new Tessera("Matematica", "A1");
bool inserita = orario.Set(Posizione(0, 0), t);
```

L'esempio presuppone che la cella sia vuota. Se l'inserimento viene rifiutato, `Orario` non assume la proprietà di `t`: chi lo ha creato deve occuparsi della memoria non trasferita.

### Attenzione: `Get()` restituisce un indirizzo, non una copia

Il valore restituito da `Get()` è di tipo `Tessera*`: chi lo riceve può raggiungere **la stessa tessera** conservata nell'orario. Non viene creato un secondo oggetto. Questo evita copie, ma introduce un rischio: se la classe `Tessera` espone metodi che modificano i suoi attributi, il chiamante può modificare direttamente i dati dell'orario senza passare dai metodi di `Orario`.

Inoltre, un puntatore restituito da `Get()` può diventare **pendente** dopo la distruzione dell'orario. Anche dopo uno scambio, il puntatore continua a indicare la stessa tessera, ma la tessera potrebbe trovarsi in una cella diversa. Perciò, nel primo prototipo, considera il puntatore restituito da `Get()` come un accesso temporaneo per consultare la tessera: non conservarlo per utilizzarlo dopo la fine della vita dell'orario.

**Esempio di lettura:**

```cpp
Tessera* t = orario.Get(Posizione(0, 0));
if (t != nullptr) {
    std::cout << t->GetMateria() << std::endl;
}
```

**Esempio del nucleo di `ScambiaCelle()`** (da completare nel relativo metodo):

```cpp
int g1 = prima.GetGiorno();
int h1 = prima.GetOra();
int g2 = seconda.GetGiorno();
int h2 = seconda.GetOra();

Tessera* temporanea = celle[g1][h1];
celle[g1][h1] = celle[g2][h2];
celle[g2][h2] = temporanea;
```

In questo caso vengono scambiati **gli indirizzi**, non gli oggetti presenti nell'heap.

## 7. Le classi dell'Engine

### 7.1 Classe `ValidatoreOrario`

Il validatore controlla che nessun giorno contenga una cella vuota seguita da una cella occupata.

**File `Engine/ValidatoreOrario.h`**

```cpp
#pragma once

#include "../Model/Orario.h"

class ValidatoreOrario {
public:
    bool SenzaBuchi(Orario& orario);
};

```

### Perché `SenzaBuchi(Orario& orario)` riceve un riferimento?

Esistono tre modi da confrontare:

```cpp
bool SenzaBuchi(Orario orario);   // per valore: crea una copia
bool SenzaBuchi(Orario* orario);  // tramite puntatore: passa un indirizzo
bool SenzaBuchi(Orario& orario);  // per riferimento: usa l'oggetto esistente
```

- **Per valore:** verrebbe creata una copia di `Orario`. Copiare la matrice di puntatori non duplica le tessere: i due orari finirebbero per puntare agli stessi oggetti. Alla distruzione delle due copie si rischierebbe di liberare più volte le medesime tessere. Per questo motivo nel progetto la copia di `Orario` non è consentita.
- **Tramite puntatore:** evita la copia, ma richiede la sintassi `orario->Get(...)` e il controllo che `orario` non sia `nullptr`. È utile quando l'assenza dell'oggetto è una possibilità da rappresentare; qui, invece, il validatore deve sempre ricevere un orario esistente.
- **Per riferimento:** evita la copia, usa la sintassi più semplice `orario.Get(...)` e richiede che il chiamante fornisca un oggetto valido. È quindi la scelta più adatta per questo metodo.

Il riferimento **non impedisce di modificare** l'orario: il validatore deve rispettare la regola progettuale di consultare le celle senza alterarle. Nel prototipo non introduciamo ancora la parola chiave `const`.

**Da realizzare in `ValidatoreOrario.cpp`:** due cicli annidati e una variabile booleana per ricordare se, nel giorno corrente, è già stata incontrata una cella vuota.

```text
per ogni giorno:
    trovataCellaVuota = false
    per ogni ora:
        se la cella è vuota:
            trovataCellaVuota = true
        altrimenti, se trovataCellaVuota è true:
            restituisci false
restituisci true
```

**Domanda guida:** perché `trovataCellaVuota` deve essere riportata a `false` all'inizio di ogni nuova giornata?

### 7.2 Classe `MotoreScambi`

Il motore prova a effettuare uno scambio e ne controlla il risultato utilizzando il validatore.

**File `Engine/MotoreScambi.h`**

```cpp
#pragma once

#include "../Model/Orario.h"
#include "ValidatoreOrario.h"

class MotoreScambi {
private:
    ValidatoreOrario validatore;

public:
    bool Scambia(Orario& orario,
                 Posizione prima,
                 Posizione seconda);
};

```

**Da realizzare in `MotoreScambi.cpp`:**

1. Richiama `orario.ScambiaCelle(prima, seconda)` per effettuare lo scambio provvisorio.
2. Richiama `validatore.SenzaBuchi(orario)`.
3. Se l'orario è valido, restituisce `true` e mantiene la nuova disposizione.
4. Altrimenti richiama una seconda volta `ScambiaCelle()` sulle stesse posizioni, ripristina l'orario iniziale e restituisce `false`.

**Perché il ripristino funziona?** Se si scambiano due puntatori e poi si scambiano nuovamente gli stessi due puntatori, ciascuno torna nella cella di partenza.

## 8. L'interfaccia Console

Per questo prototipo l'interfaccia non richiede una classe: puoi realizzarla con funzioni ordinarie in `main.cpp`.

```text
====================================
       GESTIONE ORARIO CLASSE
====================================
1 - Visualizza orario
2 - Visualizza una tessera
3 - Scambia due celle
4 - Verifica validità dell'orario
0 - Esci
Scelta: _
```

### Funzioni suggerite

Le seguenti dichiarazioni sono una possibile organizzazione del programma; **non è necessario creare un file `.h` per `main.cpp`**.

```cpp
void CaricaOrarioEsempio(Orario& orario);
void VisualizzaOrario(Orario& orario);
void VisualizzaTessera(Orario& orario);
void RichiediScambio(Orario& orario, MotoreScambi& motore);
void VerificaOrario(Orario& orario, ValidatoreOrario& validatore);
void MostraMenu();
```

La funzione `main()` crea gli oggetti necessari, carica l'orario iniziale e ripete la visualizzazione del menu finché l'utente non sceglie `0`.

### Caricamento iniziale

Prepara 34 tessere con una distribuzione di `7 + 7 + 7 + 7 + 6` lezioni. Le ore finali non utilizzate devono rimanere vuote.

Per ogni lezione puoi utilizzare uno schema simile:

```cpp
Tessera* nuova = new Tessera("Italiano", "A2");
orario.Set(Posizione(0, 1), nuova);
```

Quando il programma sarà funzionante, potrai sostituire le istruzioni ripetute con cicli e array di nomi delle materie.

### Lettura degli indici

Per gli utenti è più naturale indicare «lunedì = 1» e «prima ora = 1». Prima di costruire una `Posizione`, controlla che il giorno sia tra 1 e 5 e che l'ora sia tra 1 e 8, poi sottrai 1 a entrambi i valori.

### Comportamento dei comandi

| Comando | Azione |
|---|---|
| `1` | Stampa la tabella settimanale, indicando le celle vuote |
| `2` | Chiede giorno e ora e visualizza materia e aula, se presenti |
| `3` | Chiede due posizioni e mostra se lo scambio è stato autorizzato o rifiutato |
| `4` | Controlla l'intero orario e comunica se è valido |
| `0` | Termina il programma |

## 9. Come si svolge uno scambio

```text
Utente seleziona due posizioni
                |
                v
       MotoreScambi::Scambia
                |
                v
     ScambiaCelle (provvisorio)
                |
                v
       Validatore::SenzaBuchi
             /     \
          valido  non valido
            |         |
         conferma   ScambiaCelle
            |       (ripristino)
            |         |
            +----+----+
                 |
                 v
           Risultato al menu
```

Lo scambio non deve creare nuove tessere e non deve distruggere quelle esistenti: cambia soltanto la loro collocazione nella matrice.

## 10. Organizzazione dei file

```text
OrarioScolasticoCPP/
|
+-- Model/
|   +-- Tessera.h
|   +-- Tessera.cpp
|   +-- Posizione.h
|   +-- Posizione.cpp
|   +-- Orario.h
|   +-- Orario.cpp
|
+-- Engine/
|   +-- ValidatoreOrario.h
|   +-- ValidatoreOrario.cpp
|   +-- MotoreScambi.h
|   +-- MotoreScambi.cpp
|
+-- main.cpp
```

I cinque file `.h` riportati in questo documento sono le **interfacce da utilizzare**. I cinque file `.cpp` e `main.cpp` sono **da scrivere**: il documento non ne fornisce l'implementazione completa.

## 11. Fasi di realizzazione

| Fase | Attività | Verifica prima di proseguire |
|---|---|---|
| **1. Comprensione del problema** | Studia il vincolo e disegna un orario 5×8 | Sai riconoscere un orario valido e uno non valido |
| **2. Classi elementari** | Crea `Tessera.cpp` e `Posizione.cpp` | Riesci a costruire oggetti e leggere i loro attributi |
| **3. Matrice di puntatori** | Realizza il costruttore di `Orario`, `Get()` e `Set()` | Tutte le celle partono da `nullptr`; puoi inserire e leggere tessere |
| **4. Memoria e scambio** | Realizza il distruttore e `ScambiaCelle()` | Sai disegnare gli indirizzi prima e dopo lo scambio |
| **5. Validazione** | Realizza `SenzaBuchi()` | Il metodo distingue i casi validi da quelli non validi |
| **6. Motore** | Realizza `Scambia()` con controllo e ripristino | Uno scambio rifiutato non cambia l'orario |
| **7. Menu Console** | Realizza `main.cpp` e le funzioni di interazione | Tutte le opzioni del menu funzionano |
| **8. Collaudo** | Esegui i test e analizza la gestione della memoria | Il programma si comporta correttamente e libera le risorse |

## 12. Casi di prova

Esegui i test partendo ogni volta dall'orario iniziale, salvo dove indicato diversamente.

| Test | Operazione | Risultato atteso |
|---|---|---|
| T1 | Visualizza l'orario iniziale | 7+7+7+7+6 lezioni; nessun buco |
| T2 | Scambia due celle occupate | Scambio accettato; le lezioni cambiano posto |
| T3 | Scambia lunedì 7ª ora con venerdì 7ª ora vuota | Scambio accettato |
| T4 | Scambia lunedì 3ª ora con venerdì 7ª ora vuota | Scambio rifiutato |
| T5 | Visualizza dopo T4 | Orario identico a quello prima del tentativo |
| T6 | Scambia due celle vuote | Nessuna modifica; orario ancora valido |
| T7 | Inserisci un giorno o un'ora fuori intervallo | Il menu rifiuta l'input |
| T8 | Esci dal programma | Si attiva il distruttore di `Orario` |

Per T3 e T4 utilizza due esecuzioni separate del programma, così le modifiche del primo test non influenzano il secondo.

## 13. Vantaggi e limiti della soluzione con puntatori

Una matrice di puntatori permette di rappresentare una cella vuota con `nullptr` e una cella occupata con l'indirizzo di un oggetto. Scambiare due lezioni significa scambiare due indirizzi, senza copiare materia e aula.

Questa soluzione è utile per esercitarsi con puntatori e memoria dinamica, ma richiede attenzione: bisogna sapere chi possiede ogni oggetto, evitare che più celle ne condividano involontariamente l'indirizzo e assicurarsi che la memoria venga liberata correttamente.

La scelta dei puntatori è **intenzionalmente didattica**. Non significa che sia l'unica rappresentazione possibile o sempre la più semplice per un programma reale.

## 14. Domande di riflessione — I vantaggi dei puntatori

Rispondi motivando le tue affermazioni e utilizzando, quando utile, un disegno dello stack e dell'heap.

1. Perché una matrice di `Tessera*` permette di rappresentare facilmente sia celle occupate sia celle vuote? Qual è il ruolo di `nullptr`?
2. Durante `ScambiaCelle()`, che cosa viene scambiato realmente? Disegna la memoria prima e dopo lo scambio di una cella occupata con una vuota.
3. Perché passare `Orario&` a `SenzaBuchi()` è preferibile al passaggio per valore? In questo caso, quale differenza pratica esiste rispetto al passaggio tramite `Orario*`?

## 15. Domande di riflessione — I rischi della memoria dinamica

1. Perché il distruttore deve visitare tutte le celle? Che cosa accadrebbe se non liberasse le tessere create con `new`?
2. Che cosa potrebbe succedere se due celle contenessero lo stesso puntatore a `Tessera` e il distruttore provasse a liberare entrambi gli elementi?
3. Qual è il pericolo di `Get()` che restituisce `Tessera*` invece di una copia di `Tessera`? Considera sia la possibilità di **modificare indirettamente l'oggetto** sia il rischio di conservare un **puntatore pendente**.
4. Che cosa succede se `Set()` rifiuta una tessera creata con `new` e il chiamante perde il suo indirizzo? Chi è responsabile di quell'oggetto?
5. Perché la copia automatica di un oggetto `Orario` è pericolosa quando la matrice contiene puntatori a oggetti dinamici?

---

**Consegna finale:** un programma C++ Console funzionante, suddiviso in Model, Engine e interfaccia, con i file `.cpp` realizzati da te, i casi di prova verificati e una breve spiegazione della gestione dei puntatori e della memoria dinamica.