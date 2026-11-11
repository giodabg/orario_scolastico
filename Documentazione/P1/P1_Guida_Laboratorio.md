# P1 — Gestire le aule dell'orario con `std::vector`

**Guida operativa di laboratorio per gli studenti — versione 1.0 (11 novembre 2026)**  
**Classe:** quarta Informatica  
**Ambiente:** Visual Studio, progetto console C++  
**Punto di partenza:** il proprio progetto `OrarioClasse` al termine della P0  
**Esempio illustrato dal docente:** `OrarioDocenteCPP`, con `Classe` ed `ElencoClassi`

## Obiettivo

Il programma della P0 rappresenta l'orario settimanale di una classe: ogni `Tessera` indica materia, docente e sigla dell'aula. Ora vogliamo **memorizzare le informazioni delle aule** utilizzate nell'orario: sigla, tipologia ed edificio.

Il docente risolve un problema analogo: nel proprio orario gestisce un elenco delle **classi** in cui insegna. Gli studenti realizzano invece un elenco delle **aule** utilizzate dalla classe.

**Regola fondamentale:** non cambiare la matrice `Tessera* celle[5][8]`, né le funzioni di scambio già funzionanti. Il nuovo archivio è una struttura aggiuntiva, non una sostituzione dell'orario.

### Cosa sapremo fare al termine

- Usare `std::vector` per memorizzare oggetti per valore.
- Aggiungere elementi con `push_back`, contarli con `size`, consultarli con un indice.
- Cercare un'aula per sigla ed evitare duplicati.
- Incapsulare il vettore in una classe `ElencoAule`.
- Collegare l'archivio ai dati iniziali e al menu del programma.
- Distinguere gli oggetti contenuti nel vettore dalle tessere allocate con `new`.

---

## 1. Analizziamo il problema

Una lezione contiene, per esempio:

```text
Materia: Informatica
Docente: Rossi
Aula: INF1
```

La stringa `"INF1"` identifica l'aula, ma non dice se sia un laboratorio né in quale edificio si trovi. Per queste informazioni serve un archivio.

| Sigla | Tipologia | Edificio |
|---|---|---|
| INF1 | Laboratorio | Lotto 2 |
| A101 | Aula | Lotto 1 |
| DADA03 | Aula DADA | Lotto 3 |

**Attività 1.** Esamina le tessere del tuo orario di classe e annota tutte le **sigle diverse** delle aule utilizzate. Se `INF1` compare in cinque lezioni, nell'archivio dovrà comparire **una sola volta**.

**Domande:** perché non serve un oggetto `Aula` per ogni lezione? Perché non conviene fissare in anticipo un numero massimo di aule?

### Confronto tra i contenitori

| Orario P0 | Archivio P1 |
|---|---|
| `Tessera* celle[5][8]` | `std::vector<Aula>` |
| 40 posizioni prestabilite | Numero variabile di elementi |
| Ogni cella contiene un puntatore o `nullptr` | Ogni elemento è un oggetto `Aula` |
| `Orario` elimina le tessere possedute | Il vettore gestisce gli oggetti contenuti |

**Verifica 1.** Spiega perché la presenza di ore libere non richiede di inserire oggetti vuoti nel vettore delle aule.

---

## 2. Prepariamo il progetto in Visual Studio

1. Apri la soluzione o il progetto console C++ utilizzato nella P0.
2. Sincronizza il repository secondo la procedura Git già utilizzata in laboratorio, senza sovrascrivere le tue modifiche locali.
3. Compila ed esegui il programma **prima** di modificarlo: il menu, la visualizzazione e lo scambio devono funzionare.
4. Individua le cartelle `Model`, `Engine` e i file `DatiEsempio`, `InterfacciaConsole`, `main.cpp`. I nomi possono differire nel tuo progetto: mantieni l'organizzazione già realizzata.
5. Aggiungi alla cartella `Model` i nuovi file `Aula.h`, `Aula.cpp`, `ElencoAule.h`, `ElencoAule.cpp`. In Visual Studio assicurati che i file `.cpp` siano inclusi nel progetto.

**Verifica 2.** Compila nuovamente. Per ora i nuovi file possono contenere soltanto `#pragma once` negli header e i relativi `#include` nei sorgenti. Non modificare `Orario` o `Tessera`.

---

## 3. Progettiamo la classe `Aula`

Ogni aula deve contenere:
- `sigla`: per esempio `"INF1"`;
- `tipologia`: `"Aula"`, `"Laboratorio"` oppure `"Aula DADA"`;
- `edificio`: `"Lotto 1"`, `"Lotto 2"` oppure `"Lotto 3"`.

Il docente ha realizzato una classe `Classe` con attributi differenti: sigla, indirizzo e numero di studenti. Il vostro compito è **progettare una classe nuova**, non rinominare meccanicamente quella del docente.

**Attività 3.** Scrivi `Aula.h` con attributi privati, costruttore con tre parametri e tre metodi di lettura.

Come riferimento per la sintassi dei metodi di sola lettura:

```cpp
std::string GetSigla() const;
```

**Attività 4.** Implementa il costruttore e i metodi in `Aula.cpp`. Per verificare la classe, crea temporaneamente un oggetto:

```cpp
Aula laboratorio("INF1", "Laboratorio", "Lotto 2");
```

Stampa i tre valori utilizzando i metodi `Get`, poi rimuovi il codice temporaneo di prova dal `main`.

**Verifica 3.** La classe `Aula` non deve utilizzare `cin` o `cout`. Perché?

---

## 4. Il primo `std::vector`: oggetti memorizzati per valore

Per utilizzare il contenitore:

```cpp
#include <vector>
```

Esempio indipendente dall'orario:

```cpp
std::vector<Aula> aule;
aule.push_back(Aula("INF1", "Laboratorio", "Lotto 2"));
aule.push_back(Aula("A101", "Aula", "Lotto 1"));

int numero = static_cast<int>(aule.size());
```

`push_back` aggiunge un oggetto al vettore; `size` indica quanti oggetti contiene. In questo esempio il vettore memorizza **copie degli oggetti**, non puntatori ad essi. Non bisogna chiamare `delete` sugli elementi del vettore.

Si può accedere a un elemento tramite indice:

```cpp
std::string siglaPrimaAula = aule[0].GetSigla();
```

L'indice `0` identifica il primo elemento. `operator[]` **non controlla automaticamente** che l'indice sia valido.

**Attività 5.** Realizza un breve test temporaneo con tre aule. Stampa il numero di elementi e le loro sigle usando un ciclo `for`.

**Verifica 4.** Che cosa rappresenta `aule.size()` dopo tre inserimenti? Quali indici sono validi? Perché non devi accedere a `aule[3]`?

---

## 5. Incapsuliamo il vettore in `ElencoAule`

Il vettore non deve essere accessibile direttamente dall'interfaccia console. Creiamo una classe che offra soltanto le operazioni necessarie.

**Attività 6.** In `ElencoAule.h` dichiara questa interfaccia:

```cpp
#pragma once
#include <vector>
#include <string>
#include "Aula.h"

class ElencoAule {
private:
    std::vector<Aula> aule;

public:
    bool Aggiungi(const Aula& aula);
    int Cerca(std::string sigla) const;
    int GetNumeroAule() const;
    const Aula* Get(int indice) const;
};
```

**Significato dei metodi:**

| Metodo | Comportamento richiesto |
|---|---|
| `Aggiungi` | Inserisce una copia dell'aula, se la sigla non è vuota e non è già presente; restituisce `true` o `false` |
| `Cerca` | Cerca per sigla e restituisce l'indice; `-1` se assente |
| `GetNumeroAule` | Restituisce il numero di elementi |
| `Get` | Restituisce un puntatore di sola lettura all'aula richiesta, oppure `nullptr` se l'indice non è valido |

Il parametro `const Aula& aula` permette di usare l'oggetto passato senza copiarlo come parametro e senza modificarlo. **È `push_back` a memorizzarne una copia nel vettore.**

---

## 6. Implementiamo le operazioni del contenitore

**Attività 7 — Conteggio.** Implementa `GetNumeroAule` usando `size()`. Poiché `size()` restituisce un tipo intero senza segno, nella soluzione del docente si utilizza:

```cpp
return static_cast<int>(aule.size());
```

**Attività 8 — Ricerca.** Implementa `Cerca` con un ciclo `for`: confronta la sigla richiesta con `aule[indice].GetSigla()`; se trovi una corrispondenza, restituisci l'indice. Se il ciclo termina senza trovare nulla, restituisci `-1`.

Schema dell'algoritmo:

```text
per ogni indice valido:
    se la sigla dell'aula corrente è uguale alla sigla cercata:
        restituisci indice
restituisci -1
```

**Attività 9 — Inserimento.** Implementa `Aggiungi` secondo questi passi:

1. Se la sigla dell'aula è vuota, restituisci `false`.
2. Se `Cerca` trova già quella sigla, restituisci `false`.
3. Inserisci l'aula con `push_back`.
4. Restituisci `true`.

Non occorre utilizzare `new` per aggiungere un oggetto al vettore.

**Attività 10 — Accesso controllato.** Implementa `Get`: verifica che l'indice non sia negativo e sia minore del numero di aule. Se non è valido, restituisci `nullptr`; altrimenti restituisci l'indirizzo dell'elemento richiesto.

**Attenzione:** `Get` non restituisce una copia. Il puntatore è **non proprietario** e di sola lettura. Non chiamare `delete` su di esso e non conservarlo dopo un'aggiunta al vettore: `push_back` può spostare gli elementi e invalidare i puntatori precedentemente ottenuti.

**Verifica 5.** Esegui almeno queste prove prima di modificare la console: inserimento di due aule diverse, rifiuto di una sigla ripetuta, ricerca presente e assente, `Get(-1)` e `Get(numeroAule)`. Verifica che entrambi gli indici non validi restituiscano `nullptr`.

---

## 7. Carichiamo l'elenco iniziale delle aule

La classe `DatiEsempio` della P0 costruisce le tessere. Ora deve poter popolare anche l'archivio delle aule.

**Attività 11.** Aggiungi in `DatiEsempio.h` la dichiarazione di un metodo statico che riceva `ElencoAule&` e restituisca `bool`. Come riferimento, il docente utilizza:

```cpp
static bool CaricaClassi(ElencoClassi& elenco);
```

Nel vostro progetto il nome del metodo dovrà riflettere le **aule**, non le classi.

**Attività 12.** In `DatiEsempio.cpp`, costruisci un oggetto `Aula` per ogni **sigla distinta effettivamente utilizzata** dalle tessere dell'orario di classe. Usa `Aggiungi` e controlla gli esiti.

Esempio di una singola istruzione:

```cpp
elenco.Aggiungi(Aula("INF1", "Laboratorio", "Lotto 2"));
```

L'esempio illustra la sintassi, ma nel caricamento definitivo occorre verificare il valore restituito.

**Vincolo di coerenza iniziale:** ogni sigla di aula presente nelle tessere deve essere trovata nell'archivio. Non è richiesto ricavare automaticamente l'elenco scorrendo la matrice: il caricamento può usare dati predisposti.

**Verifica 6.** Conta le sigle diverse delle tessere e confrontale con `GetNumeroAule()`. Per i dati iniziali i due numeri devono coincidere, e ogni sigla deve essere ricercabile.

---

## 8. Colleghiamo l'archivio a `main`

La P0 crea l'orario, carica le tessere e avvia l'interfaccia. Ora serve anche un oggetto `ElencoAule`.

**Attività 13.** Modifica `main.cpp` seguendo questa sequenza:

1. Crea l'oggetto `Orario` come nella P0.
2. Carica le tessere con `DatiEsempio`.
3. Crea l'oggetto `ElencoAule`.
4. Carica le aule iniziali, controllando eventuali errori.
5. Passa **entrambi** gli oggetti al costruttore di `InterfacciaConsole`.
6. Avvia `Esegui()`.

Non spostare nel `main` il menu o la ricerca. L'oggetto `ElencoAule` deve restare in vita mentre l'interfaccia lo utilizza.

**Attività 14.** Aggiorna il costruttore di `InterfacciaConsole` affinché riceva anche un riferimento a `ElencoAule`. Memorizzalo in un attributo riferimento, inizializzato nella lista di inizializzazione del costruttore, come già fatto per `Orario&` nella P0.

**Verifica 7.** Il programma compila; la console può utilizzare sia l'orario sia l'elenco delle aule, senza creare copie degli archivi.

---

## 9. Aggiungiamo tre operazioni al menu

Mantieni le opzioni 1–4 già funzionanti nella P0 e aggiungi:

```text
5 - Visualizza elenco aule
6 - Cerca un'aula
7 - Aggiungi un'aula
0 - Esci
```

**Attività 15 — Visualizza.** Realizza un metodo privato `VisualizzaAule` in `InterfacciaConsole`. Usa `GetNumeroAule` e `Get` per stampare sigla, tipologia ed edificio di ciascuna aula.

**Attività 16 — Cerca.** Realizza un metodo privato `CercaAula`. Leggi una sigla, chiama `Cerca` sull'archivio e visualizza i dati dell'aula, oppure comunica che non è presente.

**Attività 17 — Aggiungi.** Realizza un metodo privato `AggiungiAula`. Chiedi all'utente sigla, tipologia ed edificio; crea un oggetto `Aula` e passa l'oggetto a `ElencoAule::Aggiungi`. Comunica l'esito. Per tipologie ed edifici composti da più parole, puoi presentare scelte numeriche e convertirle in stringhe, riutilizzando il metodo `LeggiIntero` della P0.

**Attività 18.** Aggiorna `MostraMenu`, l'intervallo accettato da `LeggiIntero` per la scelta e i casi dello `switch` in `Esegui`.

**Verifica 8.** Le nuove operazioni funzionano; le precedenti opzioni 1–4 continuano a funzionare. Le funzioni di input/output rimangono nell'interfaccia, non in `Aula` o `ElencoAule`.

**Precisazione:** dopo l'avvio, il menu può aggiungere anche un'aula non ancora utilizzata nelle tessere. L'archivio iniziale rappresenta esattamente le aule utilizzate; la sincronizzazione automatica dopo inserimenti e modifiche è fuori dagli obiettivi P1.

---

## 10. Collaudo finale

Esegui le prove in ordine, registrando l'esito.

| Prova | Operazione | Risultato atteso |
|---|---|---|
| P1-01 | Avvia il programma | Orario P0 e archivio aule caricati |
| P1-02 | Visualizza le aule | Una riga per sigla, senza duplicati |
| P1-03 | Cerca un'aula presente | Mostra sigla, tipologia ed edificio |
| P1-04 | Cerca una sigla assente | Segnala che non è presente |
| P1-05 | Aggiungi una sigla nuova | Inserimento riuscito; conteggio +1 |
| P1-06 | Aggiungi la stessa sigla | Inserimento rifiutato; conteggio invariato |
| P1-07 | Chiama `Get(-1)` e `Get(GetNumeroAule())` in una prova di codice | Entrambe le chiamate restituiscono `nullptr` |
| P1-08 | Cerca tutte le sigle delle tessere iniziali | Tutte presenti nell'archivio |
| P1-09 | Visualizza l'orario e una tessera | Stessi risultati della P0 |
| P1-10 | Esegui uno scambio valido e uno rifiutato | Scambio valido applicato; quello rifiutato non modifica l'orario |
| P1-11 | Esegui la verifica dell'orario | Comportamento invariato rispetto alla P0 |

Per le prove che richiedono chiamate dirette ai metodi, puoi usare temporaneamente poche istruzioni in `main` e poi rimuoverle prima della consegna.

---

## 11. Analizziamo la gestione della memoria

### Matrice dell'orario

```text
Orario
  celle[0][0] ----> Tessera creata con new
  celle[0][1] ----> Tessera creata con new
  celle[0][2] ----> nullptr

Il distruttore di Orario elimina le tessere possedute.
```

### Vettore delle aule

```text
ElencoAule
  vector<Aula> aule
    [0] Aula("INF1", ...)
    [1] Aula("A101", ...)
    [2] Aula("DADA03", ...)

Gli oggetti sono elementi del contenitore, non puntatori creati con new.
```

**Attività 19.** Rispondi: quale classe è responsabile della deallocazione delle tessere? Perché non scriviamo un ciclo con `delete` per liberare le aule? Che differenza c'è tra `const Aula&` e `const Aula*`?

---

## 12. Domande di riflessione

1. Perché `ElencoAule` contiene `std::vector<Aula>` anziché un array di dimensione fissa?
2. Perché il vettore è un attributo `private`?
3. Perché `Aggiungi` rifiuta una sigla già presente?
4. Perché `Cerca` restituisce `-1` quando non trova un'aula?
5. Perché `Get` restituisce `nullptr` per un indice non valido?
6. Perché il puntatore restituito da `Get` non deve essere conservato dopo una possibile `push_back`?
7. Che differenza c'è tra un oggetto `Aula` memorizzato per valore e un puntatore `Tessera*`?
8. Perché la ricerca di un'aula non deve essere implementata dentro `InterfacciaConsole`?

---

## 13. Checklist di consegna

- [ ] La P0 continua a funzionare senza modifiche alla matrice `Tessera* celle[5][8]`.
- [ ] Sono presenti `Aula.h`, `Aula.cpp`, `ElencoAule.h` ed `ElencoAule.cpp`.
- [ ] `Aula` contiene sigla, tipologia ed edificio, con costruttore e metodi di lettura.
- [ ] `ElencoAule` contiene un `std::vector<Aula>` privato.
- [ ] Sono implementati `Aggiungi`, `Cerca`, `GetNumeroAule` e `Get`.
- [ ] Le sigle duplicate sono rifiutate e gli indici non validi sono gestiti.
- [ ] I dati iniziali rappresentano le aule effettivamente utilizzate nell'orario.
- [ ] La console offre visualizzazione, ricerca e inserimento delle aule.
- [ ] Non sono stati introdotti `new` e `delete` per gli elementi del vettore.
- [ ] Sono state eseguite e documentate le prove di collaudo.
- [ ] Il progetto compila ed è eseguibile in Visual Studio.
- [ ] Le modifiche sono state salvate nel repository secondo il flusso Git utilizzato in classe.

## Consegna

Consegnare il progetto C++ funzionante, i quattro nuovi file di modello, le modifiche ai file di avvio e interfaccia e una breve tabella con l'esito delle prove P1-01–P1-11.

**Fase successiva P2:** approfondiremo come definire e preservare le condizioni di validità del modello. Non è richiesto nella P1 aggiornare automaticamente l'elenco delle aule quando cambia il contenuto delle tessere.

---

**Nota didattica:** gli esempi completi della soluzione guida riguardano `Classe` ed `ElencoClassi` nel programma del docente. La progettazione di `Aula` ed `ElencoAule` rimane responsabilità degli studenti.
