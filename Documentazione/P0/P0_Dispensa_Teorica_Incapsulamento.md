# P0 — Dalla funzione alla classe: incapsulamento e responsabilità

**Dispensa per lo studio individuale — Informatica, classe quarta**  
**Progetto:** Orario scolastico in C++  
**Versione:** 1.0 — 4 novembre 2026

## Obiettivi di apprendimento

Al termine dello studio dovresti saper:

- distinguere una funzione libera da un metodo e spiegare quando conviene introdurre una classe;
- riconoscere la responsabilità di una classe e distinguere modello, interfaccia utente e dati iniziali;
- usare `private` e `public` per proteggere lo stato interno di un oggetto;
- spiegare come una classe possa collaborare con un'altra tramite un riferimento;
- individuare chi possiede un oggetto creato con `new` e chi deve eseguire `delete`;
- riconoscere il problema della copia superficiale di oggetti contenenti puntatori;
- spiegare perché un'operazione rifiutata non deve lasciare modifiche parziali.

**Prerequisiti:** dichiarazione e definizione di classi, file `.h` e `.cpp`, costruttori, distruttori, puntatori, riferimenti, matrici e funzioni.

---

## 1. Il problema: un `main` che fa troppe cose

Considera un programma che gestisce un orario scolastico. In una prima versione il file `main.cpp` può contenere funzioni per leggere numeri, mostrare il menu, stampare l'orario, creare lezioni di esempio e richiedere scambi.

Questa soluzione può funzionare, ma presenta un limite: **nello stesso file sono concentrate responsabilità differenti**. Se cambia il modo di leggere i dati, potremmo dover intervenire vicino al codice che stampa la tabella. Se cambiano le lezioni di esempio, modifichiamo ancora `main.cpp`.

La difficoltà non dipende dal numero di righe in sé, ma dal fatto che il programma non rende evidente **quale parte deve occuparsi di ciascun problema**.

Una prima organizzazione è la seguente:

| Componente | Responsabilità | Esempi |
|---|---|---|
| `Tessera` | Rappresentare una lezione | Materia, classe, aula |
| `Posizione` | Rappresentare uno slot | Giorno e ora |
| `Orario` | Conservare e gestire le celle | `Get`, `Set`, `ScambiaCelle` |
| `DatiEsempio` | Costruire un orario iniziale | Creare e inserire tessere |
| `InterfacciaConsole` | Dialogare con l'utente | Menu, lettura, stampa |
| `ValidatoreOrario` | Controllare una regola | `SenzaBuchi` |
| `MotoreScambi` | Tentare una modifica controllata | Scambio, controllo, ripristino |
| `main` | Avviare l'applicazione | Creazione oggetti e chiamata a `Esegui` |

**Principio fondamentale:** una classe dovrebbe avere una responsabilità principale riconoscibile. Non significa che debba avere un solo metodo: può avere più metodi che collaborano allo stesso scopo.

### Esempio: due tipi di cambiamento

Se decidiamo di visualizzare l'orario in forma diversa, dovremmo modificare soprattutto `InterfacciaConsole`. Se cambiamo il modo in cui le tessere sono conservate, dovremmo modificare soprattutto `Orario`. Le altre classi dovrebbero continuare a usare i metodi pubblici già disponibili.

Questa separazione rende il codice più comprensibile, modificabile e verificabile.

## 2. Funzione libera e metodo di una classe

Una **funzione libera** non appartiene a una classe:

```cpp
void MostraSaluto() {
    std::cout << "Benvenuto!\n";
}
```

Un **metodo** è una funzione dichiarata all'interno di una classe. Può accedere agli attributi dell'oggetto:

```cpp
class Contatore {
private:
    int valore;

public:
    Contatore();
    void Incrementa();
    int GetValore();
};
```

Nel file `Contatore.cpp`:

```cpp
#include "Contatore.h"

Contatore::Contatore() : valore(0) {}

void Contatore::Incrementa() {
    valore++;
}

int Contatore::GetValore() {
    return valore;
}
```

L'operatore `::` indica che la definizione appartiene alla classe `Contatore`.

Non è necessario trasformare *tutte* le funzioni libere in metodi. Conviene farlo quando le funzioni condividono uno stato o appartengono chiaramente a una stessa responsabilità. Nel nostro progetto le funzioni del menu condividono l'accesso a un `Orario`: per questo ha senso riunirle in `InterfacciaConsole`.

## 3. Incapsulamento: proteggere lo stato interno

L'**incapsulamento** consiste nel raccogliere dati e operazioni in un componente e nel controllare come il resto del programma può utilizzarlo.

In C++ le parole chiave `private` e `public` permettono di definire quali membri siano accessibili dall'esterno:

```cpp
class Registro {
private:
    int numeroLezioni;

public:
    Registro();
    int GetNumeroLezioni();
    void AggiungiLezione();
};
```

Un'altra parte del programma non può scrivere direttamente `registro.numeroLezioni = -10;`, perché l'attributo è privato. Può invece usare i metodi pubblici.

**Perché è importante?** Un oggetto deve poter difendere le proprie regole. Per esempio, una posizione dell'orario è valida solo se il giorno è compreso tra 0 e 4 e l'ora tra 0 e 7. La classe `Orario` deve controllare questi valori prima di accedere alla matrice.

### L'interfaccia pubblica è un contratto

I metodi pubblici rappresentano ciò che gli altri componenti possono chiedere a una classe. Gli attributi privati e gli algoritmi interni rappresentano *come* la classe realizza il servizio.

```cpp
Tessera* Get(Posizione posizione);
bool Set(Posizione posizione, Tessera* tessera);
```

Chi usa `Orario` non dovrebbe conoscere il nome `celle` né accedere direttamente agli elementi della matrice. Questa distinzione diventerà particolarmente utile nella P1, quando potremo modificare la rappresentazione interna mantenendo operazioni simili.

**Attenzione:** il semplice uso di `private` non garantisce un incapsulamento perfetto. Se un metodo restituisce un puntatore modificabile a un oggetto interno, il chiamante potrebbe modificarlo senza passare da controlli specifici. Torneremo su questo rischio nel paragrafo 7.

## 4. Oggetti che collaborano: riferimenti e costruttori

La classe `InterfacciaConsole` deve poter consultare e modificare un orario, ma non deve crearne una copia. Può quindi conservare un **riferimento** all'oggetto `Orario` esistente:

```cpp
class InterfacciaConsole {
private:
    Orario& orario;

public:
    InterfacciaConsole(Orario& orario);
    void Esegui();
};
```

Il costruttore inizializza il riferimento:

```cpp
InterfacciaConsole::InterfacciaConsole(Orario& orario)
    : orario(orario) {
}
```

Nel `main`:

```cpp
Orario orario;
InterfacciaConsole interfaccia(orario);
interfaccia.Esegui();
```

`interfaccia` lavora sullo **stesso oggetto** `orario` creato nel `main`: non ne esiste una seconda copia.

### Perché un riferimento e non un parametro per valore?

Se il costruttore ricevesse `Orario` per valore, sarebbe necessaria una copia. Nel prototipo P0 la copia è disabilitata, perché l'orario possiede puntatori a tessere allocate dinamicamente. Il riferimento evita la copia ed esprime che l'interfaccia lavora su un oggetto già esistente.

### Durata degli oggetti

Un riferimento **non rende proprietario** chi lo conserva. `InterfacciaConsole` non deve chiamare `delete` sull'orario. È necessario però che l'oggetto `Orario` rimanga in vita per tutto il tempo in cui l'interfaccia lo utilizza.

Nel `main` mostrato, `orario` è costruito prima di `interfaccia` e viene distrutto dopo di essa: la durata è corretta.

## 5. Separare dati di esempio e interfaccia utente

Una classe come `DatiEsempio` serve a costruire lo stato iniziale del programma senza leggere nulla dalla tastiera:

```cpp
class DatiEsempio {
public:
    static bool Carica(Orario& orario);
};
```

La parola `static` significa che il metodo appartiene alla classe e può essere chiamato senza costruire un oggetto `DatiEsempio`:

```cpp
DatiEsempio::Carica(orario);
```

Il metodo riceve l'orario per riferimento, crea tessere e le inserisce con `Set`.

**Perché non inserire il caricamento in `InterfacciaConsole`?** Perché il caricamento di dati predefiniti non richiede alcuna interazione con l'utente. In futuro potremmo sostituire `DatiEsempio` con un caricamento da file senza modificare il menu.

### Due viste, due significati della tessera

Nel prototipo **orario docente**, ogni tessera deve indicare almeno *materia* e *classe*: «Informatica in 4AI». Nel prototipo **orario classe**, la tessera deve indicare almeno *materia* e *docente*: «Informatica con Rossi».

Le strutture sono simili, ma non identiche. Questa differenza è voluta: costringe a comprendere il problema anziché copiare meccanicamente una soluzione.

## 6. Matrice di puntatori e gestione della memoria

La rappresentazione scelta per la P0 è:

```cpp
Tessera* celle[5][8];
```

Questa dichiarazione crea una **matrice di 5 × 8 puntatori**, non una matrice bidimensionale allocata dinamicamente. La matrice stessa fa parte dell'oggetto `Orario`; sono le singole tessere a essere create dinamicamente con `new`.

### Inizializzazione

Il costruttore assegna `nullptr` a tutte le celle:

```cpp
Orario::Orario() {
    for (int giorno = 0; giorno < 5; giorno++) {
        for (int ora = 0; ora < 8; ora++) {
            celle[giorno][ora] = nullptr;
        }
    }
}
```

`nullptr` rappresenta una cella vuota. Non bisogna lasciare puntatori non inizializzati.

### Allocazione e proprietà

```cpp
Tessera* t = new Tessera("Informatica", "4AI", "A1");
```

`new` crea una tessera nell'area di memoria dinamica e restituisce il suo indirizzo. Il puntatore `t` permette di accedervi.

Nel nostro progetto `Orario::Set` segue una regola precisa:

- se l'inserimento riesce, `Orario` **acquisisce la proprietà** della tessera;
- se l'inserimento fallisce, la tessera resta di proprietà del chiamante.

Perciò:

```cpp
Tessera* t = new Tessera("Informatica", "4AI", "A1");
if (!orario.Set(Posizione(0, 0), t)) {
    delete t;
}
```

Il chiamante elimina la tessera soltanto quando `Set` non l'ha acquisita. Se `Set` riesce, non deve eliminarla: lo farà `Orario`.

### Distruttore

Quando un oggetto `Orario` termina la sua vita, il suo distruttore percorre la matrice ed elimina tutte le tessere possedute:

```cpp
Orario::~Orario() {
    for (int giorno = 0; giorno < 5; giorno++) {
        for (int ora = 0; ora < 8; ora++) {
            delete celle[giorno][ora];
        }
    }
}
```

In C++ `delete nullptr` è consentito e non produce effetti.

**Regola:** lo stesso oggetto dinamico deve essere eliminato una sola volta. Il metodo `Set` deve quindi evitare che lo stesso puntatore sia memorizzato in due celle differenti.

## 7. Il rischio di `Get`: puntatore, non copia

Supponiamo che `Get` restituisca:

```cpp
Tessera* t = orario.Get(Posizione(0, 0));
```

`t` non contiene una copia della tessera: contiene l'indirizzo della tessera conservata nell'orario.

Di conseguenza:

```cpp
// ERRORE: la tessera appartiene all'orario.
delete t;
```

Dopo questa istruzione la matrice dell'orario conserva un **puntatore non valido** (dangling pointer). Una successiva lettura potrebbe provocare comportamento indefinito e il distruttore rischierebbe una seconda deallocazione.

Un altro rischio riguarda la modifica dei dati interni. Se la classe `Tessera` avesse metodi `Set...`, il codice esterno potrebbe modificare una tessera senza chiedere autorizzazione a `Orario`.

Nella P0 manteniamo `Get` come nel prototipo per osservare il problema. Una possibile evoluzione futura è restituire un puntatore a oggetto `const` quando è necessaria soltanto la lettura, oppure progettare operazioni pubbliche più controllate.

### Puntatori e durata

Un puntatore ottenuto con `Get` è utilizzabile solo finché l'oggetto a cui punta esiste. Non deve essere conservato dopo la distruzione dell'orario. Lo scambio delle celle sposta i puntatori, non distrugge le tessere.

## 8. Copia superficiale e doppia deallocazione

Considera una classe che possiede un puntatore:

```cpp
class Scatola {
private:
    int* numero;
public:
    Scatola() { numero = new int(10); }
    ~Scatola() { delete numero; }
};
```

Se scriviamo:

```cpp
Scatola a;
Scatola b = a;
```

la copia implicita copia il **valore del puntatore**, non l'intero allocato. I due oggetti finiscono quindi per puntare allo stesso intero. Quando vengono distrutti, entrambi tentano di eliminarlo: è un errore.

Una rappresentazione semplificata:

```text
Oggetto a: numero ----+
                       +----> [10]  (un solo intero dinamico)
Oggetto b: numero ----+
```

La stessa difficoltà si presenterebbe copiando un `Orario` che possiede tessere dinamiche.

### Soluzione scelta per P0

```cpp
Orario(const Orario&) = delete;
Orario& operator=(const Orario&) = delete;
```

Queste dichiarazioni impediscono rispettivamente la costruzione per copia e l'assegnazione per copia. Non sono l'unica soluzione possibile: si potrebbe implementare una copia profonda, ma richiederebbe allocare nuove tessere e definire attentamente il comportamento in caso di errore. Per il prototipo iniziale la copia disabilitata è una scelta semplice e sicura.

## 9. Lo scambio come esempio di collaborazione tra classi

Il menu raccoglie due posizioni; non deve però conoscere l'algoritmo di controllo.

La sequenza è:

1. `InterfacciaConsole` legge le due posizioni.
2. `MotoreScambi` verifica che siano valide e distinte.
3. `MotoreScambi` chiede a `Orario` di scambiare le celle.
4. `ValidatoreOrario` controlla il vincolo.
5. Se il vincolo non è rispettato, `MotoreScambi` esegue nuovamente lo scambio per ripristinare la situazione precedente.
6. `InterfacciaConsole` comunica all'utente l'esito.

Il controllo scelto per P0 è volutamente elementare: **in una giornata non devono esserci celle vuote prima di una lezione successiva**. Le ore libere finali sono ammesse.

### Esempio

```text
Prima:  [A] [B] [C] [vuoto]
Dopo:   [A] [vuoto] [C] [B]
```

Il secondo stato contiene un buco: lo scambio deve essere rifiutato e il primo stato ripristinato.

Questo comportamento illustra un principio importante: **un'operazione rifiutata non deve modificare lo stato del programma**. Lo approfondiremo nella P2, quando studieremo in modo più sistematico vincoli e invarianti.

### Perché scambiare i puntatori?

Il metodo `ScambiaCelle` può scambiare gli indirizzi contenuti nella matrice, senza creare o distruggere tessere:

```cpp
Tessera* temporanea = celle[g1][o1];
celle[g1][o1] = celle[g2][o2];
celle[g2][o2] = temporanea;
```

È necessario che le posizioni siano valide prima di usare gli indici. Dopo lo scambio la proprietà delle tessere resta alla classe `Orario`.

## 10. Il nuovo ruolo di `main`

Un programma ben organizzato può avere un `main` molto breve:

```cpp
int main() {
    Orario orario;

    if (!DatiEsempio::Carica(orario)) {
        return 1;
    }

    InterfacciaConsole interfaccia(orario);
    interfaccia.Esegui();
    return 0;
}
```

`main` si occupa di **costruire e collegare i componenti** e avviare il programma. Non contiene più l'algoritmo di stampa, la lettura dei numeri o il caricamento delle tessere.

Il codice non è automaticamente migliore solo perché `main` è corto: è migliore se le responsabilità sono state distribuite in modo coerente e i metodi pubblici sono chiari.

---

# Verifica autonoma

Svolgi gli esercizi senza consultare il codice del progetto. Confronta poi le risposte con i suggerimenti finali.

## A. Domande di comprensione

**A1.** Spiega con parole tue la differenza tra *responsabilità di una classe* e *metodo di una classe*. Fornisci un esempio tratto dal progetto.

**A2.** Perché `InterfacciaConsole` deve utilizzare `Orario::Get` anziché accedere direttamente alla matrice `celle`?

**A3.** Qual è la differenza tra `Orario& orario` e `Orario orario` come parametro di una funzione? Perché nel progetto si preferisce il primo?

**A4.** Perché `DatiEsempio::Carica` può essere dichiarato `static`? Quale sarebbe un possibile motivo per trasformarlo in un metodo non statico in una versione futura?

**A5.** Chi deve eseguire `delete` dopo una chiamata riuscita a `Set`? E dopo una chiamata fallita?

**A6.** Perché `Get` non deve essere interpretato come una funzione che restituisce una copia della tessera?

**A7.** Quale problema si verifica copiando superficialmente un `Orario` che possiede puntatori a tessere dinamiche?

**A8.** Perché `MotoreScambi` deve ripristinare lo stato precedente se il validatore rifiuta lo scambio?

## B. Analisi del codice

**B1. Individua il problema di memoria.**

```cpp
Tessera* t = new Tessera("Informatica", "4AI", "A1");
orario.Set(Posizione(0, 0), t);
delete t;
```

Descrivi due casi: inserimento riuscito e inserimento fallito. Riscrivi il frammento correttamente.

**B2. Individua il problema di durata.**

```cpp
Tessera* CercaLezione() {
    Orario orario;
    DatiEsempio::Carica(orario);
    return orario.Get(Posizione(0, 0));
}
```

Perché il puntatore restituito non può essere usato in sicurezza?

**B3. Valuta la copia.**

```cpp
Orario primo;
DatiEsempio::Carica(primo);
Orario secondo = primo;
```

Con la versione P0 fornita, il codice compila? Spiega il motivo. Che cosa potrebbe accadere senza la protezione contro la copia?

**B4. Osserva gli indici.** Un utente inserisce «giorno 5, ora 8». Quali indici devono essere utilizzati nella matrice? Che cosa succede se vengono usati direttamente 5 e 8?

## C. Esercizi di progettazione

**C1.** Disegna uno schema delle relazioni tra `main`, `InterfacciaConsole`, `DatiEsempio`, `Orario`, `MotoreScambi` e `ValidatoreOrario`. Per ogni collegamento scrivi quale servizio viene richiesto.

**C2.** Il programma deve mostrare quante ore di Informatica compaiono nell'orario. Proponi in quale classe inseriresti il calcolo e in quale classe inseriresti la stampa del risultato. Motiva la scelta. Non è necessario scrivere codice.

**C3.** Gli studenti devono realizzare l'orario di una classe. Scrivi gli attributi essenziali della loro `Tessera` e spiega perché differiscono da quelli della `Tessera` dell'orario docente.

**C4.** Progetta tre casi di prova per il metodo `Set`: un inserimento valido e due inserimenti rifiutati. Per ogni prova specifica stato iniziale, operazione e risultato atteso.

**C5.** Scrivi uno pseudocodice per `MotoreScambi::Scambia` che garantisca il ripristino dopo una verifica fallita. Indica le condizioni da controllare prima di accedere alla matrice.

## D. Piccolo esercizio pratico indipendente

Realizza una classe `Biblioteca` che conserva il numero di libri disponibili:

- attributo privato `int disponibili`;
- costruttore che inizializza il numero a zero;
- metodo `Aggiungi(int quantita)` che accetta soltanto quantità positive;
- metodo `Presta()` che riduce il numero soltanto se esiste almeno un libro;
- metodo `GetDisponibili()` per consultare il numero.

Scrivi `Biblioteca.h`, `Biblioteca.cpp` e un `main.cpp` che verifichi un prestito riuscito e uno rifiutato. **Vincolo:** `main` non deve modificare direttamente l'attributo `disponibili`.

---

## Autocorrezione: indicazioni essenziali

*Leggi questa sezione solo dopo aver svolto gli esercizi.*

- **A1–A2:** una responsabilità è un compito complessivo, i metodi sono operazioni che lo realizzano; la matrice privata va utilizzata tramite i metodi pubblici.
- **A3:** il passaggio per valore richiede una copia, quello per riferimento utilizza l'oggetto esistente; in P0 la copia di `Orario` è disabilitata.
- **A4:** `Carica` non richiede attributi di un oggetto `DatiEsempio`; diventerebbe non statico se dovesse utilizzare uno stato proprio, ad esempio una configurazione.
- **A5:** se `Set` riesce, libera la tessera `Orario`; se fallisce, la libera il chiamante.
- **A6–A7:** `Get` restituisce un indirizzo; la copia superficiale duplica gli indirizzi, non le tessere, e può provocare doppia deallocazione.
- **A8:** uno scambio rifiutato non deve cambiare l'orario.
- **B1:** se l'inserimento riesce, `delete t` lascia un puntatore non valido nell'orario; se fallisce, eliminare `t` è necessario. Usare `if (!orario.Set(..., t)) delete t;`.
- **B2:** l'oggetto locale `orario` viene distrutto quando la funzione termina; il puntatore restituito non è più valido.
- **B3:** la copia non compila perché è esplicitamente disabilitata; senza protezione si rischierebbe una doppia deallocazione.
- **B4:** gli indici sono `4` e `7`; usare `5` e `8` produce un accesso fuori dai limiti.
- **C2:** una soluzione ragionevole è calcolare nel modello o in un componente di analisi e stampare nell'interfaccia. L'importante è non mescolare calcolo e presentazione.
- **C3:** nell'orario docente la tessera identifica la *classe*; nell'orario classe identifica il *docente*.
- **C4:** esempi: cella libera e posizione valida → successo; cella occupata → rifiuto; indici non validi → rifiuto.
- **C5:** controllare indici e posizioni distinte, eseguire lo scambio, verificare e, se necessario, scambiare nuovamente le celle.

## Checklist finale

Prima della prossima lezione, verifica di saper rispondere senza appunti:

- [ ] So attribuire ogni funzione del prototipo alla classe più adatta.
- [ ] So distinguere `private` e `public` e spiegare perché non basta nascondere gli attributi.
- [ ] So spiegare la differenza tra riferimento, puntatore e copia nel contesto del progetto.
- [ ] So dire chi possiede ogni tessera e chi deve liberarla.
- [ ] So descrivere i rischi di `Get` e della copia superficiale.
- [ ] So descrivere lo scambio con verifica e ripristino.

**Collegamento alla fase successiva:** nella P1 studieremo come sostituire la struttura interna di `Orario` senza riscrivere il menu e le altre parti del programma.