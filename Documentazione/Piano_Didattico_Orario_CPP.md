# Progetto Orario Scolastico in C++
## Documento guida per la programmazione didattica e lo sviluppo incrementale

**Versione:** 1.1 — 11 novembre 2026  
**Destinatari:** classe quarta, indirizzo Informatica  
**Stato:** documento di riferimento; durata delle fasi da definire  
**Formato:** Markdown, da conservare nel repository e utilizzare come fonte nelle chat dedicate alle singole fasi.

---

## 1. Finalità e perimetro

Il percorso sviluppa competenze di programmazione C++ e progettazione software attraverso prototipi successivi di un gestore di orari scolastici. Il docente realizza e illustra **l'orario dal punto di vista del docente**; gli studenti progettano **l'orario dal punto di vista della classe**. I due percorsi, inizialmente indipendenti, convergono in un'applicazione integrata.

**Obiettivo finale:** gestire un numero configurabile **X di classi** e **Y di docenti**, con lezioni condivise, viste coerenti, controlli sui conflitti, scambi, annullamento, richieste di modifica e semplici analisi. Il programma **non genera automaticamente** un orario ottimale. Interfaccia della versione C++: **console**. Dopo il completamento e il collaudo del nucleo C++ è prevista una fase aggiuntiva **P10** per realizzare un'interfaccia grafica **C# / WPF**, riutilizzando `Model` ed `Engine` C++ tramite un livello di interoperabilità **C++/CLI**. La versione console rimane un prodotto completo e verificabile.

Il progetto serve a introdurre i concetti, non a massimizzare il numero di funzionalità. Ogni versione deve compilare, essere eseguibile e avere criteri di accettazione verificabili.

## 2. Competenze finali

Al termine del percorso lo studente sa:

1. Progettare classi con responsabilità definite, incapsulamento e interfacce pubbliche coerenti.
2. Spiegare e usare puntatori, riferimenti, memoria dinamica, copia e durata degli oggetti; riconoscere rischi di ownership.
3. Specificare un ADT tramite operazioni, precondizioni, postcondizioni e invarianti, distinguendolo dalla rappresentazione fisica.
4. Utilizzare contenitori STL (`vector`, `stack`, `queue` e, dove motivato, `list`, `map`, `set`) scegliendoli in base alle operazioni necessarie.
5. Progettare relazioni di composizione ed ereditarietà motivate; utilizzare classi astratte, metodi virtuali e polimorfismo.
6. Comprendere e utilizzare template di base, iteratori e algoritmi STL; ragionare sulla complessità delle operazioni principali.
7. Progettare operazioni di modifica che preservino la coerenza del modello; predisporre test e diagnosticare errori.
8. Organizzare file `.h`/`.cpp`, documentare le scelte e utilizzare Git per lo sviluppo incrementale.
9. Individuare duplicazioni fra due applicazioni e proporre componenti comuni senza forzare l'ereditarietà.
10. Nell'estensione P10, collegare un'interfaccia WPF in C# a componenti C++ nativi attraverso un bridge C++/CLI, senza duplicare la logica applicativa e rispettando la proprietà degli oggetti.

**Approfondimenti non obbligatori:** implementazione fisica di liste, pile e code con nodi; alberi binari, visite ricorsive e alberi di ricerca; strutture bilanciate. La ricorsione può essere trattata anche con esercizi autonomi.

## 3. Requisiti funzionali del prototipo finale

| ID | Requisito | Accettazione di massima |
|---|---|---|
| RF01 | Gestire X classi configurabili | Aggiunta e selezione di più classi senza modificare il codice sorgente |
| RF02 | Gestire Y docenti configurabili | Aggiunta e selezione di più docenti senza modificare il codice sorgente |
| RF03 | Rappresentare lezioni con classe, docente, materia e collocazione temporale | Dati interrogabili in modo coerente |
| RF04 | Consultare l'orario di una classe | Mostra le lezioni della classe selezionata |
| RF05 | Consultare l'orario di un docente | Mostra le lezioni del docente selezionato |
| RF06 | Garantire la coerenza delle viste | Una modifica valida è visibile in entrambe le viste |
| RF07 | Rifiutare sovrapposizioni | Un docente o una classe non può avere due lezioni nello stesso slot |
| RF08 | Tentare scambi di lezioni | Operazioni valide applicate; operazioni non valide senza modifiche parziali |
| RF09 | Annullare le modifiche | Ripristino corretto delle ultime modifiche riuscite |
| RF10 | Gestire richieste di scambio | Elaborazione FIFO delle richieste |
| RF11 | Verificare vincoli estensibili | Nuovi vincoli aggiungibili tramite polimorfismo |
| RF12 | Eseguire ricerche e statistiche | Risultati verificabili su dati di prova |

**Fuori ambito obbligatorio per P0–P9:** generazione automatica dell'orario, ottimizzazione combinatoria, interfaccia grafica, database, gestione avanzata delle aule, distribuzione su rete. L'interfaccia grafica è prevista separatamente nella **P10**, senza modificare i requisiti di completamento della versione console.

### 3.2 Requisiti dell'estensione WPF (P10)

| ID | Requisito | Accettazione di massima |
|---|---|---|
| RG01 | Visualizzare in WPF gli orari di classi e docenti | Le due viste mostrano i dati del medesimo `OrarioIstituto` C++ |
| RG02 | Richiedere scambi dall'interfaccia grafica | Le richieste sono delegate al motore C++; esiti e messaggi sono visualizzati in C# |
| RG03 | Preservare invarianti e annullamento | Scambi rifiutati senza effetti; annullamenti coerenti in entrambe le viste |
| RG04 | Isolare l'interoperabilità | Il bridge C++/CLI espone dati e operazioni .NET senza trasferire puntatori nativi alla UI |
| RG05 | Gestire durata e proprietà delle risorse | Nessuna doppia deallocazione; rilascio controllato del modello nativo |
| RG06 | Mantenere indipendente il nucleo C++ | `Model` ed `Engine` non dipendono da WPF né duplicano la logica in C# |
 Le aule possono restare attributo facoltativo delle lezioni.

### 3.1 Invarianti finali del modello

- Ogni lezione fa riferimento a una classe e a un docente esistenti.
- Ogni lezione occupa uno slot valido (giorno, ora).
- Nello stesso slot una classe non può avere più di una lezione.
- Nello stesso slot un docente non può avere più di una lezione.
- Le viste per classe e per docente derivano da **un'unica fonte di verità**, non da copie indipendenti da sincronizzare manualmente.
- Una modifica rifiutata non cambia lo stato del sistema.
- L'annullamento ripristina uno stato coerente.

**Da precisare:** settimana didattica, numero di ore al giorno, rappresentazione delle ore libere, disciplina delle lezioni con compresenza, eventuali lezioni su più ore, semantica esatta dello scambio.

## 4. Strategia didattica: due progetti paralleli

**Docente — `OrarioDocente`:** esempio guidato centrato sugli impegni settimanali di un insegnante; una tessera identifica almeno classe e materia. Problemi tipici: ore libere, buchi, ore per classe.

**Studenti — `OrarioClasse`:** sviluppo autonomo centrato sulle lezioni settimanali di una classe; una tessera identifica almeno docente e materia. Problemi tipici: distribuzione delle materie, ore per docente, vincoli della classe.

I problemi devono essere **analoghi ma non identici**: stessa competenza informatica, dati e condizioni differenti. Il codice del docente è un riferimento ragionato, non una soluzione da copiare sostituendo nomi.

**Convergenza in tre momenti:** (1) sviluppo separato; (2) confronto delle interfacce, delle duplicazioni e delle differenze; (3) integrazione su modello condiviso `OrarioIstituto` con due viste. Non decidere in anticipo che le due classi debbano condividere una superclasse: valutare composizione, funzioni generiche e, solo quando giustificata, ereditarietà.

## 5. Regole tecniche e di progressione

- Standard C++ da fissare prima della produzione del codice: **proposta C++17**, salvo vincoli dell'ambiente scolastico.
- Preferire file `.h` e `.cpp`, con `#pragma once` negli header.
- Nelle prime fasi mantenere l'esperienza con matrice dinamica manuale, `new`/`delete`, costruttori, distruttori e gestione della copia; poi motivare la transizione a `std::vector` e RAII.
- Evitare di restituire puntatori modificabili a oggetti interni senza esplicitare rischi di aliasing, invalidazione e violazione degli invarianti.
- Usare `const` e riferimenti quando appropriato; introdurre `unique_ptr` soltanto dopo ownership e polimorfismo.
- Non anticipare nei tutorial costrutti non ancora insegnati, salvo segnalarli esplicitamente come codice fornito e non richiesto.
- Per ogni fase: interfacce essenziali stabili, test minimi, codice compilabile, commit/tag di versione.
- `Model` ed `Engine` non devono contenere `cin`/`cout` per l'interazione con l'utente né dipendere da WPF; input, output e presentazione spettano a `InterfacciaConsole` nella versione C++ e alla UI WPF nella P10.
- Preparare l'interoperabilità mantenendo API applicative ben definite; nella P10 il bridge converte i dati C++ in tipi utilizzabili da .NET e non espone `Tessera*` o altri puntatori nativi all'interfaccia C#.
- Pile e code: **ADT e uso STL obbligatori**; implementazione con nodi facoltativa. `std::list` non va inserita artificiosamente se `vector` è più adatto.
- Alberi binari: approfondimento autonomo; `map` e `set` non espongono l'albero sottostante.

## 6. Quadro generale delle fasi

| Fase | Prototipo funzionante | Nucleo principale | Dipende da |
|---|---|---|---|
| P0 | Orario singolo visualizzabile | Classi, puntatori, memoria | Prerequisiti C++ |
| P1 | Orario singolo come ADT con `vector` | Astrazione, incapsulamento, STL | P0 |
| P2 | Scambi con verifiche di base | Algoritmi, invarianti, riferimenti | P1 |
| P3 | Vincoli estensibili | Composizione, ereditarietà, polimorfismo | P2 |
| P4 | Cronologia e annullamento | ADT pila, `std::stack` | P2–P3 |
| P5 | Richieste di scambio | ADT coda, `std::queue` | P3–P4 |
| P6 | Ricerca e analisi | Iteratori, algoritmi STL, template | P1–P5 |
| P7 | Orari di più soggetti | Contenitori, identità, ricerca, vincoli incrociati | P6 |
| P8 | Modello istituto e viste integrate | Riutilizzo, composizione, coerenza dei dati | P7 |
| P9 | Collaudo e consegna della versione C++ | Testing, Git, documentazione | P8 |
| P10 | Interfaccia WPF sul nucleo C++ | Eventi WPF, interoperabilità C++/CLI, gestione risorse | P9 |

La **P10** è un'estensione successiva alla consegna del programma console: non introduce una seconda implementazione di `Model` ed `Engine` in C#.

Le fasi P7 e P8 sono esplicite perché il requisito **X classi / Y docenti** non deve essere trattato come un semplice dettaglio dell'ultima versione. La distribuzione oraria resta aperta.

---

## 7. Schede delle fasi

### P0 — Orario singolo e memoria dinamica

- **Obiettivo:** costruire e visualizzare un orario settimanale di un solo soggetto.
- **Problema concreto:** memorizzare e leggere tessere per giorno e ora.
- **Nuovo concetto:** gestione di oggetti dinamici e responsabilità della memoria.
- **Prerequisiti:** classi di base, vettori, funzioni, puntatori e riferimenti introduttivi.
- **Competenze:** realizzare classi, allocare/deallocare correttamente, riconoscere copie superficiali.
- **Argomenti:** `Tessera`, `Orario`, matrice dinamica, costruttore, distruttore, `get`/`set`, copia o copia esplicitamente disabilitata.
- **Prototipo docente:** orario di un docente, tessere con classe e materia; stampa settimanale.
- **Compito studenti:** orario di una classe, tessere con docente e materia; stessa struttura di accesso ma dati diversi.
- **Vincoli:** nessuna STL sostitutiva della matrice in questa fase; non esporre modifiche incontrollate della memoria interna.
- **Verifica oggettiva:** inizializzazione, lettura/scrittura, controllo degli indici, distruzione senza errori, comportamento di copia dichiarato e testato.
- **Output:** due applicazioni console autonome; base per P1.

### P1 — ADT Orario e `std::vector`

- **Obiettivo:** mantenere le operazioni pubbliche sostituendo la rappresentazione interna.
- **Problema concreto:** eliminare complessità e rischi della gestione manuale della matrice.
- **Nuovo concetto:** ADT, indipendenza dall'implementazione, contenitore STL.
- **Prerequisiti:** P0 e significato di incapsulamento.
- **Competenze:** definire invarianti e contratti, usare `vector`, distinguere interfaccia da implementazione.
- **Argomenti:** `std::vector<Tessera>`, indicizzazione giorno/ora, `const`, validazione indici, valore/riferimento.
- **Prototipo docente:** refactoring di `OrarioDocente` con comportamento invariato.
- **Compito studenti:** refactoring di `OrarioClasse`, mantenendo compatibili i test già realizzati.
- **Vincoli:** nessuna nuova funzionalità di business; verificare la sicurezza di `get`.
- **Verifica oggettiva:** test P0 ancora superati, nessun `new[]`/`delete[]` nella rappresentazione dell'orario, accessi invalidi gestiti.
- **Output:** orari singoli basati su STL; base per P2.

### P2 — Scambio e invarianti

- **Obiettivo:** effettuare scambi controllati tra due posizioni.
- **Problema concreto:** impedire modifiche incoerenti dell'orario.
- **Nuovo concetto:** precondizioni, postcondizioni, operazioni atomiche a livello applicativo.
- **Prerequisiti:** P1.
- **Competenze:** progettare un algoritmo di modifica e preservare lo stato in caso di errore.
- **Argomenti:** indici, `scambia`, verifica preliminare o modifica temporanea con ripristino, esito dell'operazione.
- **Prototipo docente:** scambio di impegni e controllo elementare dei buchi.
- **Compito studenti:** scambio di lezioni con controllo semplice della giornata scolastica.
- **Vincoli:** definire esplicitamente il significato di scambio; non introdurre ancora gerarchie di vincoli.
- **Verifica oggettiva:** scambio valido applicato; posizioni invalide o vincolo violato lasciano l'orario identico.
- **Output:** prototipi modificabili; base per P3.

### P3 — Vincoli, ereditarietà e polimorfismo

- **Obiettivo:** aggiungere controlli senza riscrivere l'algoritmo di scambio.
- **Problema concreto:** vincoli differenti e destinati a crescere.
- **Nuovo concetto:** classe astratta, metodo virtuale, overriding, polimorfismo.
- **Prerequisiti:** P2, classi e riferimenti/puntatori.
- **Competenze:** distinguere composizione e relazione is-a; progettare un'interfaccia polimorfica.
- **Argomenti:** `Vincolo`, `verifica(const Orario&)`, `virtual`, `override`, distruttore virtuale, contenitore di vincoli, ownership.
- **Prototipo docente:** `VincoloSenzaBuchi` e `VincoloMassimoOre` per docente.
- **Compito studenti:** vincoli per classe, per esempio numero massimo di ore di una materia al giorno e assenza di intervalli interni non consentiti.
- **Vincoli:** non creare gerarchie di tessere senza una relazione is-a reale; chiarire durata e proprietà dei vincoli.
- **Verifica oggettiva:** due vincoli distinti applicati; nuovo vincolo aggiunto senza cambiare le classi di vincolo già esistenti; scambi rifiutati senza effetti.
- **Output:** sistema di controlli estensibile; base per P4–P5.

### P4 — Annullamento con pila

- **Obiettivo:** annullare gli ultimi scambi riusciti.
- **Problema concreto:** recuperare lo stato precedente dopo una modifica.
- **Nuovo concetto:** ADT pila, disciplina LIFO, `std::stack`.
- **Prerequisiti:** P2–P3.
- **Competenze:** scegliere una pila, registrare modifiche e implementare il ripristino.
- **Argomenti:** `push`, `top`, `pop`, `empty`, classe `Modifica`, cronologia.
- **Prototipo docente:** annullamento sequenziale di scambi di impegni.
- **Compito studenti:** annullamento sequenziale di scambi di lezioni, con casi di test diversi.
- **Vincoli:** solo modifiche riuscite nella pila; annullare non deve registrare una nuova modifica.
- **Verifica oggettiva:** due scambi seguiti da due annullamenti ripristinano l'ordine inverso; pila vuota gestita; stato sempre coerente.
- **Output:** prototipi con cronologia; base per P5.

### P5 — Richieste di scambio con coda

- **Obiettivo:** gestire richieste in ordine di arrivo.
- **Problema concreto:** separare richiesta e applicazione della modifica.
- **Nuovo concetto:** ADT coda, disciplina FIFO, `std::queue`.
- **Prerequisiti:** P3–P4.
- **Competenze:** accodare, processare, rifiutare o accettare richieste mantenendo l'ordine.
- **Argomenti:** `push`, `front`, `pop`, `empty`, `RichiestaScambio`, esito.
- **Prototipo docente:** richieste di modifica agli impegni del docente.
- **Compito studenti:** richieste di modifica delle lezioni di classe.
- **Vincoli:** ogni richiesta è elaborata una volta; richiesta rifiutata non modifica l'orario e non entra nella cronologia delle modifiche riuscite.
- **Verifica oggettiva:** ordine FIFO osservabile; richiesta non valida rifiutata; richiesta valida applicata e annullabile.
- **Output:** gestione delle richieste; base per P6.

### P6 — Ricerche, statistiche e template

- **Obiettivo:** interrogare gli orari e riutilizzare algoritmi.
- **Problema concreto:** evitare codice ripetuto per conteggi e ricerche.
- **Nuovo concetto:** iteratori, algoritmi STL, template di funzione.
- **Prerequisiti:** P1–P5 e uso di `vector`.
- **Competenze:** usare `find_if`, `count_if` e altri algoritmi appropriati; scrivere una funzione template semplice; stimare costi elementari.
- **Argomenti:** iteratori, predicati, lambda se già introdotte, `template<typename T>`, complessità di ricerca lineare; eventuale `map` per aggregazioni.
- **Prototipo docente:** conteggio delle ore per classe e ricerca degli impegni.
- **Compito studenti:** conteggio delle ore per materia e ricerca delle lezioni per docente.
- **Vincoli:** preferire algoritmi standard; evitare template artificiosi e ordinamenti che alterino la collocazione temporale dell'orario.
- **Verifica oggettiva:** conteggi su dataset noto, ricerche presenti/assenti, almeno un riuso generico motivato.
- **Output:** funzioni di analisi; base per P7.

### P7 — Più docenti e più classi

- **Obiettivo:** estendere ciascun prototipo da un soggetto a più soggetti.
- **Problema concreto:** scegliere, cercare e gestire X classi o Y docenti.
- **Nuovo concetto:** identità degli oggetti, collezioni di entità, relazioni tra dati.
- **Prerequisiti:** P6.
- **Competenze:** gestire insiemi dinamici di oggetti, identificatori stabili, ricerca e controlli incrociati.
- **Argomenti:** `vector`, eventuale `map`, identificatori, lookup, distinzione tra entità e viste.
- **Prototipo docente:** gestione di Y docenti con selezione dell'orario individuale.
- **Compito studenti:** gestione di X classi con selezione dell'orario individuale.
- **Vincoli:** niente dimensioni hardcoded; identificatori non ambigui; i dati condivisi possono essere ancora duplicati nei due prototipi, ma la duplicazione va evidenziata.
- **Verifica oggettiva:** almeno tre soggetti di prova per percorso, aggiunta e selezione, nessuna confusione tra identificatori.
- **Output:** due applicazioni multi-soggetto; punto di partenza per l'integrazione.

### P8 — Convergenza in `OrarioIstituto`

- **Obiettivo:** integrare le due viste in un'unica applicazione coerente.
- **Problema concreto:** la stessa lezione compare nell'orario di classe e in quello del docente; evitare divergenze.
- **Nuovo concetto:** fonte unica di verità, composizione, refactoring architetturale, riuso mirato.
- **Prerequisiti:** P7 e conoscenza delle differenze tra i due modelli.
- **Competenze:** individuare componenti comuni, separare dati e viste, scegliere composizione/ereditarietà/template motivatamente.
- **Argomenti:** `Lezione`, `OrarioIstituto`, identificatori, viste derivate, conflitti di classe/docente, operazioni transazionali elementari.
- **Prototipo docente:** guida alla costruzione del modello comune e della vista per docente.
- **Compito studenti:** integrare la vista per classe nel modello condiviso, contribuire ai test di coerenza e alle operazioni incrociate.
- **Vincoli:** una sola registrazione logica per lezione; scambi che coinvolgono docenti e classi verificati globalmente; non imporre superclasse comune se non giustificata.
- **Verifica oggettiva:** spostamento valido visibile in entrambe le viste; conflitti respinti; annullamento ripristina entrambe le viste; X e Y variabili.
- **Output:** primo programma integrato; base per P9.

### P9 — Collaudo, documentazione e consegna

- **Obiettivo:** consegnare un'applicazione integrata stabile e comprensibile.
- **Problema concreto:** dimostrare la correttezza di un sistema costruito in più fasi.
- **Nuovo concetto:** test sistematici, regressione, qualità e documentazione.
- **Prerequisiti:** P8.
- **Competenze:** costruire casi di prova, documentare invarianti, correggere difetti, organizzare la consegna Git.
- **Argomenti:** test di unità e integrazione, casi limite, debugging, README, versionamento.
- **Prototipo docente:** suite dimostrativa dei requisiti e casi di errore.
- **Compito studenti:** test indipendenti, correzioni e dimostrazione del proprio contributo al programma integrato.
- **Vincoli:** nessuna nuova funzionalità obbligatoria; priorità alla stabilità.
- **Verifica oggettiva:** matrice RF01–RF12 superata su dataset condiviso, nessuna incoerenza nota, build riproducibile e documentazione aggiornata.
- **Output:** versione finale del Gestore Orario in C++ con interfaccia console; base stabile per l'eventuale P10.

### P10 — Integrazione C++/C# e interfaccia grafica WPF

- **Obiettivo:** realizzare una GUI WPF in C# riutilizzando `Model` ed `Engine` C++ già collaudati.
- **Problema concreto:** sostituire l'interazione console con eventi e controlli grafici senza riscrivere la logica dell'orario.
- **Nuovi concetti:** interoperabilità fra codice nativo e .NET tramite C++/CLI; programmazione guidata dagli eventi.
- **Prerequisiti:** P9; nozioni di base di C#, XAML, eventi WPF e gestione delle risorse.
- **Competenze:** progettare un bridge, trasferire dati e risultati in modo sicuro, gestire la durata degli oggetti nativi, separare UI e servizi applicativi.
- **Argomenti:** libreria C++ nativa, progetto C++/CLI, classi bridge, conversione di stringhe e oggetti, code-behind WPF, gestori di eventi, rilascio delle risorse.
- **Prototipo docente:** finestra WPF per consultare l'orario dei docenti ed eseguire scambi delegati al motore C++.
- **Compito studenti:** realizzare la vista WPF degli orari delle classi, utilizzando lo stesso modello integrato e il bridge condiviso; verificare gli esiti con casi di prova differenti.
- **Vincoli:** `Model` ed `Engine` restano in C++ standard; nessuna duplicazione degli algoritmi in C#; la GUI non riceve puntatori nativi; il bridge ha una responsabilità esplicita sulla durata degli oggetti.
- **Verifica oggettiva:** RG01–RG06 superati; viste coerenti, scambi validi applicati, scambi non validi rifiutati senza modifiche, annullamento corretto, nessuna perdita o doppia liberazione di risorse nei test previsti.
- **Output:** applicazione WPF funzionante collegata al nucleo C++ e documentazione del livello di interoperabilità.

---

## 8. Schema standard per sviluppare ogni fase in una nuova chat

1. **Identificazione:** codice, titolo, versione iniziale/finale.
2. **Obiettivo:** risultato funzionale osservabile.
3. **Problema concreto:** esigenza che motiva la fase.
4. **Prerequisiti:** conoscenze e artefatti necessari.
5. **Nuovi concetti:** massimo uno o due nuclei principali.
6. **Competenze:** formulate come azioni osservabili.
7. **Argomenti:** conoscenze, costrutti e API.
8. **Prototipo guida docente:** funzionalità, classi, metodi, interfacce essenziali.
9. **Consegna studenti:** problema analogo non identico, dati e vincoli specifici.
10. **Vincoli progettuali e didattici:** cosa è consentito, richiesto o rinviato.
11. **Criteri oggettivi di verifica:** casi di test con input, esito atteso, gestione degli errori.
12. **Materiali da produrre:** dispensa, tutorial, codice iniziale, soluzione, test, esercizi.
13. **Continuità:** componenti riutilizzati e preparazione della fase seguente.
14. **Decisioni aperte:** questioni non ancora definite, senza inventare soluzioni implicite.

**Regola per le chat successive:** usare questo documento come fonte normativa del percorso; esplicitare ogni proposta di modifica e aggiornare il documento principale soltanto dopo approvazione. Non confondere una scelta proposta con una scelta approvata.

## 9. Materiali e repository

Struttura suggerita (adattabile al repository esistente):

```text
orario-scolastico/
├── README.md
├── documentazione/
│   ├── Piano_Didattico_Orario_CPP.md
│   ├── decisioni.md
│   └── fasi/
│       ├── P0/
│       ├── P1/
│       └── ...
├── cpp/
│   ├── docente/
│   ├── studenti/
│   ├── integrato/
│   └── test/
├── bridge-cpp-cli/      # collegamento tra nucleo C++ e .NET (P10)
└── csharp/              # interfaccia WPF (P10)
```

Per ogni fase: specifica in Markdown, codice docente, progetto iniziale studenti, test, soluzione commentata ed esercizi di consolidamento. Le versioni stabili devono essere riconoscibili con tag o commit; evitare di sovrascrivere senza tracciamento il codice di fasi precedenti.

## 10. Valutazione

Valutare separatamente: **correttezza funzionale**, **comprensione dei concetti**, **qualità delle interfacce e degli invarianti**, **capacità di adattamento dal progetto docente**, **testing e documentazione**. Non attribuire automaticamente maggiore valore all'uso di strutture più complesse. Le rubriche numeriche e le prove saranno definite per singola fase.

## 11. Decisioni consolidate e questioni aperte

### Decisioni consolidate

- Destinatari: quarta Informatica; linguaggio C++.
- Progetto guida: vista docente; progetto studenti: vista classe.
- Progressione incrementale con prototipi funzionanti.
- Convergenza finale in un sistema con X classi e Y docenti.
- STL come strumento principale per pile, code e contenitori; implementazioni fisiche facoltative.
- Alberi binari come approfondimento, non vincolo del progetto.
- Interfaccia console per il percorso C++ P0–P9 e gestione di un orario esistente, non generazione automatica.
- Dopo P9, estensione P10 con interfaccia C# / WPF e bridge C++/CLI, riutilizzando `Model` ed `Engine` nativi senza riscriverli.
- Documento Markdown come fonte per chat, materiali e sviluppo.

### Decisioni ancora aperte

- Standard C++ e ambiente di compilazione effettivi.
- Numero di giorni/ore e regole per gli slot non occupati.
- Significato preciso di scambio (solo posizioni? vincoli su docente/classe? eventuali lezioni multiple?).
- Insieme minimo dei vincoli obbligatori e loro ordine di introduzione.
- Modello delle aule, compresenze e lezioni di durata superiore a un'ora.
- Formato di caricamento iniziale dei dati (codice, file di testo, altro).
- Scelta definitiva di identificatori, strutture e API per il modello integrato.
- Ripartizione oraria, modalità di valutazione e livello degli approfondimenti.
- Organizzazione finale dei repository GitHub e sincronizzazione dei materiali.

## 12. Registro modifiche

| Versione | Data | Descrizione |
|---|---|---|
| 1.0 | 2026-11-04 | Prima stesura: obiettivi, requisiti, sviluppo parallelo, fasi P0–P9, integrazione finale e questioni aperte |
| 1.1 | 2026-11-11 | Aggiunta P10: GUI WPF in C#, bridge C++/CLI, riuso di Model ed Engine C++; aggiornati competenze, requisiti, regole e struttura repository |