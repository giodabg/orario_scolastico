# P0 — Dal prototipo all'orario del docente

**Guida operativa di laboratorio — versione 0.1 (4 novembre 2026)**  
**Classe:** quarta Informatica  
**Percorso guidato:** `OrarioDocenteCPP`  
**Percorso autonomo degli studenti:** orario della classe (progetto C++ già esistente)

## Obiettivo e punto di partenza

Il prototipo iniziale contiene `Tessera`, `Posizione`, `Orario`, `MotoreScambi` e `ValidatoreOrario`, ma anche otto funzioni nel file `main.cpp`: `LeggiIntero`, `LeggiPosizione`, `CaricaOrarioEsempio`, `VisualizzaOrario`, `VisualizzaTessera`, `RichiediScambio`, `VerificaOrario`, `MostraMenu`.

Il problema non è aggiungere funzionalità: è **distribuire le responsabilità tra le classi**. L'orario continua a essere rappresentato da `Tessera* celle[5][8]`, con tessere allocate dinamicamente. Lo scambio e il controllo «senza buchi» restano prototipi di prova.

Prerequisiti già acquisiti: classi, metodi, attributi privati, file `.h` e `.cpp`, costruttori, distruttori, puntatori e riferimenti. Per la teoria si farà riferimento alla dispensa di studio individuale.

## 1. Esaminiamo il `main.cpp` iniziale

**Problema.** Il `main` conosce contemporaneamente le regole di lettura da tastiera, la formattazione dell'orario, la costruzione dei dati e l'esecuzione del menu.

**Attività.** Individuare nel sorgente ciascuna funzione e assegnarle una responsabilità:

| Funzione iniziale | Destinazione | Motivo |
|---|---|---|
| `LeggiIntero` | `InterfacciaConsole` | Legge e controlla input |
| `LeggiPosizione` | `InterfacciaConsole` | Converte numeri da tastiera in indici |
| `VisualizzaOrario` | `InterfacciaConsole` | Presenta dati all'utente |
| `VisualizzaTessera` | `InterfacciaConsole` | Presenta una cella |
| `RichiediScambio` | `InterfacciaConsole` | Chiede posizioni e comunica l'esito |
| `VerificaOrario` | `InterfacciaConsole` | Mostra l'esito del validatore |
| `MostraMenu` | `InterfacciaConsole` | Presenta le opzioni |
| `CaricaOrarioEsempio` | `DatiEsempio` | Costruisce i dati iniziali, senza interazione |

**Verifica.** Spiegare perché `Orario` non dovrebbe contenere `cin` e `cout` e perché `InterfacciaConsole` non dovrebbe accedere direttamente a `celle`.

## 2. Introduciamo `InterfacciaConsole`

**Problema.** Il ciclo del menu non è responsabilità di `main`.

**Modifica.** Creare `InterfacciaConsole.h` e `InterfacciaConsole.cpp`. La classe contiene un riferimento all'orario e gli oggetti necessari per lo scambio e la verifica:

```cpp
class InterfacciaConsole {
private:
    Orario& orario;
    MotoreScambi motore;
    ValidatoreOrario validatore;
    // Metodi privati di lettura, visualizzazione e menu.
public:
    InterfacciaConsole(Orario& orario);
    void Esegui();
};
```

Il riferimento viene inizializzato dal costruttore: `InterfacciaConsole::InterfacciaConsole(Orario& orario) : orario(orario) {}`. L'interfaccia **non è proprietaria** dell'orario.

**Attività.** Trasferire per prime `MostraMenu`, `LeggiIntero`, `LeggiPosizione` e il ciclo `do ... while` in `Esegui`.

**Verifica.** Il menu compare, le scelte sono riconosciute e l'opzione 0 termina il programma. Perché `Esegui` è pubblico e `MostraMenu` è privato?

## 3. Visualizziamo senza modificare il modello

**Problema.** Le funzioni di visualizzazione devono leggere il modello, non gestirne la memoria.

**Modifica.** Trasferire `VisualizzaOrario` e `VisualizzaTessera` in `InterfacciaConsole`. Le informazioni si ottengono tramite `orario.Get(Posizione(giorno, ora))`.

```cpp
Tessera* tessera = orario.Get(Posizione(giorno, ora));
if (tessera == nullptr) {
    // Cella libera.
} else {
    // Visualizza i dati della tessera.
}
```

**Attività.** Stampare cinque giorni e otto ore. L'utente usa numeri da 1; gli indici della matrice partono da 0.

**Verifica.** Consultare una cella occupata e una libera. La visualizzazione non deve alterare l'orario.

**Attenzione.** `Get` restituisce un puntatore a una tessera interna, non una copia. Non chiamare `delete` sul risultato e non conservarlo oltre la vita dell'orario.

## 4. Distinguiamo orario docente e orario classe

**Problema.** Nel prototipo originale `Tessera` contiene materia e aula; per un orario docente occorre sapere **in quale classe** si tiene la lezione.

**Modifica guidata.** Nella versione del docente la tessera contiene `materia`, `classe`, `aula`. Si aggiornano costruttore e metodi `GetMateria`, `GetClasse`, `GetAula`. La stampa settimanale mostra materia e classe.

**Attività autonoma.** Nel progetto dell'orario della classe gli studenti devono rappresentare `materia`, `docente`, `aula`, adattando costruttore, metodi e stampa. Non copiare meccanicamente `GetClasse`: i dati rispondono a domande diverse.

**Verifica.** Nel programma guida è possibile conoscere la classe di ogni lezione; nel programma degli studenti il docente di ogni lezione.

## 5. Introduciamo `DatiEsempio`

**Problema.** Il caricamento iniziale non è una funzione di interfaccia: crea oggetti e popola il modello.

**Modifica.** Creare `DatiEsempio.h` e `DatiEsempio.cpp`, con una semplice funzione statica:

```cpp
class DatiEsempio {
public:
    static bool Carica(Orario& orario);
};
```

`Carica` costruisce tessere con `new`, poi chiama `orario.Set(...)`. Nel prototipo guida le lezioni si riferiscono a classi diverse; i dati sono **fittizi** e servono a esercitare il programma, non a riprodurre un orario scolastico realistico.

**Regola di proprietà.** `Set` restituisce `true` solo quando acquisisce la tessera. Se restituisce `false`, il chiamante deve eliminarla:

```cpp
Tessera* tessera = new Tessera("Informatica", "4AI", "A1");
if (!orario.Set(Posizione(0, 0), tessera)) {
    delete tessera;
}
```

**Verifica.** Il caricamento iniziale funziona; un inserimento rifiutato non perde memoria. Discutere perché il distruttore di `Orario` elimina le tessere, ma non deve eliminare una seconda volta quelle già liberate.

## 6. Manteniamo lo scambio come prova di collaborazione

**Problema.** La console deve *richiedere* uno scambio; il motore deve *eseguirlo* e decidere se mantenerlo.

**Modifica.** `InterfacciaConsole::RichiediScambio()` legge due posizioni e chiama `motore.Scambia(orario, prima, seconda)`.

Il motore verifica che le posizioni siano valide e distinte e che l'orario iniziale sia valido; scambia le celle, chiama `ValidatoreOrario::SenzaBuchi` e, se il controllo fallisce, ripete lo scambio per ripristinare lo stato iniziale.

**Definizione operativa del vincolo P0.** Una giornata è «senza buchi» se, dalla prima ora in poi, non compare una cella vuota prima di una lezione successiva. Le celle vuote *alla fine* della giornata sono ammesse. È un controllo deliberatamente semplificato: non pretende di descrivere tutte le situazioni reali di un docente.

**Verifica.** Scambiare due celle occupate (esito positivo); scambiare la prima e l'ottava ora del lunedì (rifiuto e ripristino). Controllare l'orario prima e dopo.

## 7. Separiamo richiesta di verifica e algoritmo

**Problema.** `VerificaOrario` mostra un messaggio; `ValidatoreOrario::SenzaBuchi` effettua il controllo.

**Modifica.** Il metodo `InterfacciaConsole::VerificaOrario()` chiama il validatore senza duplicarne il codice.

**Verifica.** L'orario iniziale è valido. Un orario costruito con una cella vuota tra due lezioni è rifiutato dal validatore.

## 8. Riduciamo `main.cpp`

Al termine il file contiene soltanto l'avvio dell'applicazione:

```cpp
int main() {
    Orario orario;
    if (!DatiEsempio::Carica(orario)) return 1;
    InterfacciaConsole interfaccia(orario);
    interfaccia.Esegui();
    return 0;
}
```

**Verifica.** Il programma compila, mostra il menu, visualizza l'orario e permette scambi e verifiche. Nessuna funzione di input/output resta nel `main`.

## 9. Criteri di conclusione della P0

- [ ] `main` crea gli oggetti e avvia il programma, senza gestire direttamente il menu.
- [ ] `InterfacciaConsole` contiene input, menu e visualizzazione.
- [ ] `DatiEsempio` crea le lezioni iniziali senza leggere dalla tastiera.
- [ ] `Orario` mantiene privata la matrice `Tessera* celle[5][8]`.
- [ ] Le tessere sono inizializzate a `nullptr` e liberate dal distruttore.
- [ ] `Set` controlla gli indici, la disponibilità della cella e la proprietà dei puntatori.
- [ ] La copia di `Orario` è esplicitamente disabilitata per evitare copie superficiali.
- [ ] Lo scambio valido modifica le celle; quello rifiutato lascia invariato l'orario.
- [ ] L'orario docente identifica la classe; quello degli studenti identifica il docente.
- [ ] Entrambi i progetti sono compilabili e verificabili separatamente.

## 10. Prove suggerite

| Caso | Azione | Risultato atteso |
|---|---|---|
| T1 | Avvio e opzione 1 | Tabella di 5 giorni × 8 ore |
| T2 | Opzione 2, lunedì ora 1 | Materia, classe e aula |
| T3 | Opzione 2, lunedì ora 8 | Cella libera |
| T4 | Opzione 3, lunedì ore 1 e 2 | Scambio accettato |
| T5 | Opzione 3, lunedì ore 1 e 8 | Scambio rifiutato; nessuna modifica |
| T6 | Opzione 4 dopo T5 | Orario ancora valido |
| T7 | Chiamata a `Set` su cella occupata | `false`; il chiamante libera la tessera |
| T8 | `Get` con indici fuori intervallo | `nullptr` |

## 11. Attività autonoma e fase successiva

Gli studenti applicano la stessa sequenza al proprio orario di classe: identificano le responsabilità, introducono l'interfaccia, separano i dati di esempio, adattano le tessere e verificano gli scambi. Il problema è analogo, ma la rappresentazione delle lezioni è diversa.

**Domande conclusive:** Chi possiede una tessera? Perché `Get` è potenzialmente pericoloso? Perché il menu non appartiene a `Orario`? Perché uno scambio rifiutato deve ripristinare lo stato precedente?

Nella **P1** sarà possibile sostituire la rappresentazione interna di `Orario` mantenendo, per quanto possibile, invariati i servizi offerti alle altre classi.

---

**Nota per il docente.** Il codice completo allegato rappresenta lo *stato finale* del laboratorio, non il punto di partenza da consegnare subito agli studenti. La guida descrive una sequenza di trasformazioni; per farle svolgere realmente in classe, iniziare dal prototipo già presente nel repository e applicare le modifiche una alla volta.
