# P1 — std::vector e archivi di oggetti: classi e aule

**Destinatari:** quarta Informatica  
**Versione:** 1.0 — 11 novembre 2026  
**Riferimento:** `Documentazione/Piano_Didattico_Orario_CPP.md`, versione 1.3  
**Prerequisito software:** prototipo P0 compilabile e funzionante.

## 1. Obiettivo e problema concreto

Estendere il programma dell'orario con un archivio di entità utilizzate nelle lezioni. Il numero di classi o aule non è noto a priori: un array a dimensione fissa richiederebbe di scegliere una capacità massima, mentre `std::vector` gestisce una sequenza di oggetti la cui dimensione può crescere.

**Vincolo centrale:** non sostituire la rappresentazione dell'orario `Tessera* celle[5][8]`. L'orario conserva la proprietà delle tessere allocate con `new`; il nuovo archivio memorizza invece **oggetti per valore**.

## 2. Nuovo concetto informatico

- `std::vector<T>` come contenitore dinamico di oggetti.
- Operazioni `push_back`, `size` e accesso indicizzato.
- Ricerca sequenziale per sigla; rifiuto dei duplicati.
- Incapsulamento: il vettore è privato e accessibile tramite metodi.
- Passaggio per `const T&`, restituzione di puntatori non proprietari e significato di `const`.

Non introdurre in questa fase smart pointer, `std::optional`, template personalizzati, iteratori espliciti, algoritmi STL avanzati o ereditarietà.

## 3. Competenze osservabili

Al termine della P1 lo studente:
1. Dichiara e utilizza un `std::vector` di oggetti per valore.
2. Implementa un archivio con inserimento, ricerca e accesso controllato.
3. Motiva il controllo delle sigle duplicate.
4. Distingue la proprietà degli oggetti nel vettore da quella delle tessere allocate dinamicamente.
5. Spiega perché un puntatore restituito a un elemento del vettore non deve essere eliminato con `delete` e può essere invalidato dopo una modifica del contenitore.
6. Integra l'archivio nella console senza trasferire input/output nelle classi del modello.
7. Verifica che le funzionalità della P0 continuino a funzionare.

## 4. Due problemi analoghi, non identici

| Aspetto | Soluzione guida docente | Compito studenti |
|---|---|---|
| Programma | `OrarioDocenteCPP` | `OrarioClasse` |
| Entità | `Classe` | `Aula` |
| Attributi | sigla, indirizzo, numero studenti | sigla, tipologia, edificio |
| Archivio | `ElencoClassi` | `ElencoAule` |
| Contenitore | `std::vector<Classe>` | `std::vector<Aula>` |
| Operazioni | aggiungi, cerca, conta, ottieni | aggiungi, cerca, conta, ottieni |
| Riferimento nelle tessere | sigla classe | sigla aula |

Per le aule considerare le tipologie **aula, laboratorio, aula DADA** e gli edifici **Lotto 1, Lotto 2, Lotto 3**. Le sigle sono identificatori testuali, non puntatori agli elementi del vettore.

## 5. Soluzione di riferimento del docente

**Nuovi file:**
- `OrarioDocenteCPP/Model/Classe.h` e `.cpp`
- `OrarioDocenteCPP/Model/ElencoClassi.h` e `.cpp`

**File aggiornati:** `DatiEsempio.h/.cpp`, `InterfacciaConsole.h/.cpp`, `main.cpp`.

**Interfaccia dell'archivio:**
- `bool Aggiungi(const Classe& classe)`: aggiunge una copia se la sigla non è vuota né duplicata.
- `int Cerca(std::string sigla) const`: restituisce l'indice o `-1`.
- `int GetNumeroClassi() const`: restituisce il numero di classi.
- `const Classe* Get(int indice) const`: restituisce un puntatore di sola lettura o `nullptr`.

Il puntatore di `Get` è **non proprietario** e valido solo finché il contenitore non viene modificato in modo da invalidarlo. Non conservarlo attraverso operazioni `Aggiungi`.

**Caricamento iniziale:** i dati di esempio contengono quattro sigle (`4AI`, `4BI`, `3AI`, `5AI`), tutte presenti nell'orario P0. L'archivio viene popolato con le informazioni anagrafiche di queste classi. Non è richiesta la derivazione automatica dell'elenco dalle tessere.

**Console:** preservare le opzioni 1–4 e aggiungere:
- 5: visualizzare l'elenco classi;
- 6: cercare una classe per sigla;
- 7: aggiungere una classe.

Le classi del modello non utilizzano `cin` o `cout`.

## 6. Sequenza di lavoro suggerita

| Passo | Attività docente | Attività studenti | Evidenza |
|---|---|---|---|
| A | Confronto tra array fisso e vettore | Prevedere le operazioni necessarie | Schema delle operazioni |
| B | Implementazione di `Classe` | Progettazione di `Aula` | Header e costruttore |
| C | `ElencoClassi` con `vector` | `ElencoAule` con `vector` | Inserimento e conteggio |
| D | Ricerca e rifiuto duplicati | Ricerca e rifiuto duplicati | Casi presenti/assenti |
| E | Integrazione con `DatiEsempio` e console | Integrazione nel proprio programma | Menu funzionante |
| F | Collaudo di regressione P0 | Test indipendenti | Esiti documentati |

## 7. Criteri oggettivi di verifica

| ID | Prova | Risultato atteso |
|---|---|---|
| T01 | Avvio del programma | Orario P0 caricato; quattro classi nell'archivio docente |
| T02 | Visualizzazione elenco | Sigla, indirizzo e numero studenti per ogni classe |
| T03 | Ricerca sigla presente (`4AI`) | Elemento trovato e attributi corretti |
| T04 | Ricerca sigla assente | Messaggio di assenza; nessuna modifica |
| T05 | Inserimento sigla nuova | Conteggio aumentato di uno |
| T06 | Inserimento sigla duplicata | Inserimento rifiutato; conteggio invariato |
| T07 | `Get` con indice negativo o oltre la dimensione | Restituisce `nullptr` |
| T08 | Confronto sigle iniziali con le tessere | Ogni sigla usata nell'orario esiste nell'archivio |
| T09 | Funzioni P0: stampa, consultazione, scambio, verifica | Nessuna regressione |
| T10 | Revisione codice | Nessun `delete` per oggetti contenuti nel `vector`; `Orario` invariato |

Gli studenti eseguono gli stessi tipi di prova su `ElencoAule` con dati propri. Non è richiesto che le sigle aggiunte manualmente siano immediatamente usate dalle tessere: l'archivio iniziale deve essere coerente con l'orario; l'invariante «esattamente e soltanto entità utilizzate» non viene mantenuto automaticamente per gli inserimenti successivi.

## 8. Materiali e output

- Questa scheda docente.
- Soluzione C++ del docente pubblicata nei file `OrarioDocenteCPP`.
- Guida laboratorio studenti: `Documentazione/P1/P1_Guida_Laboratorio.md`.
- Dispensa teorica: `Documentazione/P1/P1_Dispensa_Teorica.md`.
- Da produrre successivamente: esercizi con soluzioni separate, presentazione e piano di test dettagliato.

**Output P1:** due applicazioni console con la matrice di tessere della P0 e un archivio dinamico di oggetti per valore. **Continuità P2:** consolidamento dei contratti, degli invarianti e delle operazioni di scambio.
