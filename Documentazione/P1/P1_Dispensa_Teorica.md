# P1 — `std::vector`: archivi dinamici di oggetti in C++

**Dispensa teorica per lo studio individuale — Informatica, classe quarta**  
**Progetto:** Orario scolastico in C++  
**Versione:** 1.0 — 11 novembre 2026  
**Prerequisiti:** P0, classi, costruttori, attributi privati, metodi, array, puntatori e riferimenti.

## Obiettivi di apprendimento

Al termine della dispensa dovresti saper:
- spiegare perché un archivio con un numero variabile di elementi può essere rappresentato con `std::vector`;
- dichiarare un vettore di oggetti e utilizzare `push_back`, `size` e `operator[]`;
- distinguere **oggetti memorizzati per valore** da **puntatori a oggetti**;
- realizzare una ricerca sequenziale e impedire l'inserimento di identificativi duplicati;
- spiegare il ruolo di `const` e del passaggio per riferimento costante;
- progettare una classe che incapsula un vettore e ne controlla l'accesso;
- riconoscere il rischio di invalidazione di puntatori e riferimenti agli elementi del vettore;
- confrontare l'archivio P1 con la matrice di puntatori alle tessere della P0.

Questa dispensa spiega i concetti attraverso esempi autonomi. La **guida di laboratorio P1** descrive invece le modifiche da apportare al progetto in Visual Studio.

---

## 1. Un problema diverso dalla matrice dell'orario

L'orario settimanale della P0 ha una struttura prestabilita: cinque giorni e otto ore per giorno. Ogni cella contiene un puntatore a una `Tessera` oppure `nullptr`.

```cpp
Tessera* celle[5][8];
```

La posizione `celle[0][0]` rappresenta sempre la prima ora del lunedì. La matrice ha quindi **40 posizioni**, indipendentemente dal numero di lezioni effettivamente presenti.

Un elenco di classi o aule risponde a un problema diverso. Se un docente insegna in quattro classi, l'archivio contiene quattro elementi. Se ne viene aggiunta una quinta, deve poter crescere senza modificare il codice che dichiara l'archivio.

Un array tradizionale, per esempio `Classe classi[10];`, ha una capacità fissata in anticipo. Bisognerebbe distinguere tra posti disponibili e posti effettivamente utilizzati e controllare di non superare la capacità.

Con `std::vector` il numero degli elementi viene gestito dal contenitore.

**Attenzione:** `std::vector` non è sempre «migliore» di un array. Le due strutture rispondono a esigenze differenti. In P1 **manteniamo la matrice dell'orario** e aggiungiamo un vettore per l'archivio.

## 2. Che cos'è `std::vector`?

`std::vector` è un contenitore della libreria standard C++ che conserva una sequenza di elementi **dello stesso tipo**, accessibili mediante un indice. Il numero degli elementi può aumentare o diminuire durante l'esecuzione.

Per usarlo occorre includere l'header:

```cpp
#include <vector>
```

La dichiarazione

```cpp
std::vector<int> voti;
```

crea un vettore inizialmente vuoto di numeri interi.

Le parentesi angolari specificano il **tipo degli elementi**: `int` in questo caso. È una prima applicazione di una classe generica della libreria standard; non è ancora necessario studiare come si scrivono i template.

### Esempio completo: un vettore di interi

```cpp
#include <iostream>
#include <vector>

int main() {
    std::vector<int> voti;

    voti.push_back(7);
    voti.push_back(8);
    voti.push_back(6);

    for (int i = 0; i < static_cast<int>(voti.size()); i++) {
        std::cout << voti[i] << "\n";
    }

    return 0;
}
```

Il programma stampa:

```text
7
8
6
```

`push_back` aggiunge un elemento **alla fine**; `size` restituisce il numero degli elementi presenti; `voti[i]` accede all'elemento di indice `i`.

## 3. Dimensione, indici e capacità

Se il vettore contiene tre elementi, gli indici validi sono `0`, `1` e `2`. La sua dimensione è `3`.

```text
indice:       0       1       2
            +-------+-------+-------+
voti        |   7   |   8   |   6   |
            +-------+-------+-------+
size() = 3
```

**Regola:** un indice è valido se è maggiore o uguale a zero e strettamente minore di `size()`.

Il metodo `size()` restituisce un tipo intero senza segno, chiamato `std::size_t`. Nei semplici esempi del progetto, quando è comodo lavorare con indici `int`, usiamo esplicitamente:

```cpp
int numero = static_cast<int>(voti.size());
```

La conversione è adeguata per i piccoli archivi didattici del progetto; per contenitori molto grandi si preferisce un tipo compatibile con `size()`.

### Dimensione e capacità non sono la stessa cosa

- **Dimensione (`size`)**: quanti elementi sono effettivamente presenti.
- **Capacità (`capacity`)**: quanti elementi il vettore può contenere prima che possa essere necessario riallocare lo spazio di memorizzazione.

La capacità può essere maggiore della dimensione. Non bisogna usare gli indici fino a `capacity()`: sono validi solo quelli inferiori a `size()`.

**Errore da evitare:**

```cpp
std::vector<int> numeri;
numeri[0] = 5; // ERRORE: il vettore e' vuoto.
```

Per inserire il primo elemento si usa `numeri.push_back(5);`.

### `operator[]` non controlla i limiti

Accedere a `voti[3]` quando `size() == 3` è un accesso non valido e provoca **comportamento indefinito**. Non è garantito un messaggio di errore.

Esiste anche `at()`, che controlla i limiti e segnala l'errore con un'eccezione. Nella P1 non affrontiamo la gestione delle eccezioni: useremo controlli espliciti sugli indici.

## 4. Dal vettore di interi al vettore di oggetti

Un vettore può contenere oggetti di una classe definita da noi.

Considera questo semplice modello, indipendente dall'orario:

```cpp
#pragma once
#include <string>

class Libro {
private:
    std::string codice;
    std::string titolo;

public:
    Libro(std::string codice, std::string titolo);
    std::string GetCodice() const;
    std::string GetTitolo() const;
};
```

Nel file `Libro.cpp`:

```cpp
#include "Libro.h"

Libro::Libro(std::string codice, std::string titolo)
    : codice(codice), titolo(titolo) {}

std::string Libro::GetCodice() const {
    return codice;
}

std::string Libro::GetTitolo() const {
    return titolo;
}
```

Possiamo quindi dichiarare:

```cpp
std::vector<Libro> libri;
libri.push_back(Libro("L01", "Algoritmi"));
libri.push_back(Libro("L02", "Reti"));
```

Il tipo del vettore è `std::vector<Libro>`: **gli elementi sono oggetti `Libro`**, non indirizzi di memoria.

Per leggere il titolo del primo libro:

```cpp
std::string titolo = libri[0].GetTitolo();
```

Usiamo il punto `.` perché `libri[0]` è un oggetto. Se avessimo un puntatore `Libro*`, useremmo `->`.

## 5. Che cosa significa «memorizzare per valore»?

Osserva:

```cpp
Libro primo("L01", "Algoritmi");
std::vector<Libro> libri;
libri.push_back(primo);
```

`primo` è un oggetto; `push_back(primo)` ne inserisce **una copia** nel vettore.

Schema concettuale:

```text
Oggetto locale primo             Elementi del vector<Libro>
+-------------------------+      +-------------------------+
| codice = "L01"          |      | [0] Libro               |
| titolo = "Algoritmi"    | ---> | codice = "L01"          |
+-------------------------+ copia| titolo = "Algoritmi"    |
                                 +-------------------------+
```

L'oggetto locale e l'elemento del vettore sono **due oggetti distinti**. Modificare successivamente l'oggetto originale non modifica automaticamente la copia inserita.

Nel caso di `Libro`, gli attributi `std::string` vengono copiati correttamente. Questa è una differenza importante rispetto alla copia superficiale di una classe che possiede puntatori dinamici, discussa nella P0.

### Chi libera la memoria?

Non serve chiamare `delete` sugli elementi di `std::vector<Libro>`. Il vettore gestisce la memoria necessaria per conservare i suoi elementi e, quando viene distrutto, distrugge gli oggetti contenuti.

Non confondere:
- `std::vector<Libro>`: contiene **oggetti**;
- `std::vector<Libro*>`: contiene **puntatori**.

Un vettore di puntatori non diventa automaticamente proprietario degli oggetti puntati. **In P1 usiamo il primo caso.**

## 6. Ricerca sequenziale: trovare un elemento tramite codice

Supponiamo di voler trovare un libro a partire dal suo codice.

L'algoritmo più semplice è la **ricerca sequenziale**: esaminiamo gli elementi uno alla volta, dal primo all'ultimo, finché troviamo il codice richiesto.

```cpp
int CercaLibro(const std::vector<Libro>& libri, std::string codice) {
    for (int i = 0; i < static_cast<int>(libri.size()); i++) {
        if (libri[i].GetCodice() == codice) {
            return i;
        }
    }
    return -1;
}
```

La funzione restituisce:
- l'indice dell'elemento, se lo trova;
- `-1`, se non lo trova.

Il valore `-1` non può essere un indice valido e può quindi rappresentare in modo semplice la condizione «assente».

**Perché il ciclo termina?** L'indice aumenta a ogni iterazione e il numero di elementi è finito.

**Quanti confronti sono necessari?** Nel caso migliore basta il primo elemento. Nel caso peggiore occorre esaminare tutti gli `n` elementi. Il numero dei confronti cresce quindi, nel caso peggiore, in proporzione a `n`: questa proprietà si indica con **O(n)**. Non è necessario conoscere algoritmi di ricerca più avanzati per la P1.

### Il problema dei duplicati

Se ogni libro è identificato dal codice, non vogliamo due libri con lo stesso codice nell'archivio. Prima di inserire un elemento, possiamo cercarlo:

```text
se il codice e' vuoto:
    rifiuta
se CercaLibro trova gia' il codice:
    rifiuta
altrimenti:
    aggiungi il libro al vettore
```

Il controllo appartiene alla classe che gestisce l'archivio, non alla funzione che stampa il menu.

## 7. Incapsulare il vettore: la classe `CatalogoLibri`

Se altre parti del programma potessero modificare direttamente il vettore, potrebbero inserire codici duplicati senza controlli.

Per evitare questo problema, il contenitore diventa un attributo **privato** di una classe:

```cpp
#pragma once
#include <vector>
#include <string>
#include "Libro.h"

class CatalogoLibri {
private:
    std::vector<Libro> libri;

public:
    bool Aggiungi(const Libro& libro);
    int Cerca(std::string codice) const;
    int GetNumeroLibri() const;
    const Libro* Get(int indice) const;
};
```

Questa è l'**interfaccia pubblica** dell'archivio: definisce quali servizi possono essere richiesti, senza esporre il vettore.

Nel file `CatalogoLibri.cpp`:

```cpp
#include "CatalogoLibri.h"

int CatalogoLibri::GetNumeroLibri() const {
    return static_cast<int>(libri.size());
}

int CatalogoLibri::Cerca(std::string codice) const {
    for (int i = 0; i < GetNumeroLibri(); i++) {
        if (libri[i].GetCodice() == codice) {
            return i;
        }
    }
    return -1;
}

bool CatalogoLibri::Aggiungi(const Libro& libro) {
    if (libro.GetCodice().empty()) {
        return false;
    }

    if (Cerca(libro.GetCodice()) != -1) {
        return false;
    }

    libri.push_back(libro);
    return true;
}

const Libro* CatalogoLibri::Get(int indice) const {
    if (indice < 0 || indice >= GetNumeroLibri()) {
        return nullptr;
    }
    return &libri[indice];
}
```

**Osservazione:** l'inserimento è un'operazione controllata. Chi usa `CatalogoLibri` non può chiamare direttamente `libri.push_back` perché `libri` è privato.

Questo non significa che tutti gli archivi debbano offrire esattamente questi quattro metodi: l'interfaccia dipende dai requisiti del problema.

## 8. Passaggio per valore, riferimento e riferimento costante

Considera tre dichiarazioni di funzione:

```cpp
void Elabora(Libro libro);           // Per valore
void Modifica(Libro& libro);         // Per riferimento
void Mostra(const Libro& libro);     // Per riferimento costante
```

### Per valore

`Elabora(Libro libro)` riceve una **copia** dell'oggetto. Modificare il parametro non modifica l'originale, ma copiare può avere un costo.

### Per riferimento

`Modifica(Libro& libro)` lavora sull'oggetto originale e può modificarlo.

### Per riferimento costante

`Mostra(const Libro& libro)` lavora sull'oggetto originale **senza copiarlo** e non può modificarlo tramite quel riferimento.

Per questo il metodo `Aggiungi` usa:

```cpp
bool Aggiungi(const Libro& libro);
```

Il parametro è passato senza copia e non può essere modificato dal metodo. L'operazione `libri.push_back(libro)` effettua poi la **copia destinata al contenitore**.

### `const` dopo il nome del metodo

```cpp
int GetNumeroLibri() const;
```

Il `const` finale indica che il metodo non può modificare direttamente lo stato osservabile dell'oggetto attraverso il normale accesso ai suoi membri. Perciò può essere chiamato anche su un oggetto `const`.

Anche i metodi `GetCodice() const` e `GetTitolo() const` sono importanti: possono essere invocati su un `const Libro&` o un `const Libro*`.

**Da non confondere:** `const Libro&` indica un riferimento di sola lettura; `const Libro*` indica un puntatore attraverso il quale non si può modificare l'oggetto puntato. Sono due modi diversi di accedere a un oggetto.

## 9. Perché `Get` restituisce `const Libro*`?

Considera:

```cpp
const Libro* libro = catalogo.Get(0);

if (libro != nullptr) {
    std::cout << libro->GetTitolo() << "\n";
}
```

`Get` restituisce l'**indirizzo di un elemento interno**, non una copia. Questo evita una copia a ogni lettura e permette di rappresentare l'assenza di un elemento con `nullptr`.

Il puntatore è **non proprietario**: non bisogna scrivere `delete libro;`. Inoltre è un puntatore a `const Libro`, quindi non permette di modificare il libro tramite quel puntatore.

### Attenzione: invalidazione dopo `push_back`

Un vettore memorizza i propri elementi in una zona contigua di memoria. Quando deve crescere oltre la capacità disponibile, può allocare una zona più grande e trasferirvi gli elementi.

```text
Prima:
vector<Libro>        [ Libro 0 ][ Libro 1 ]
                          ^
                          |
                      puntatore p

Dopo una crescita con riallocazione:
vecchia zona         [ non piu' valida ]
nuova zona           [ Libro 0 ][ Libro 1 ][ Libro 2 ]

p non indica piu' un elemento valido.
```

**Regola pratica per la P1:** non conservare puntatori o riferimenti agli elementi del vettore attraverso un'operazione `Aggiungi`. `push_back` **può** invalidarli, anche se non lo fa necessariamente a ogni inserimento.

Non utilizzare un puntatore invalidato: il comportamento è indefinito. Se occorre ritrovare un elemento dopo una modifica, esegui nuovamente la ricerca tramite il suo identificativo.

## 10. Collegamento con il progetto dell'orario

Nel programma del docente la P1 introduce:

```cpp
std::vector<Classe> classi;
```

Ogni `Classe` contiene sigla, indirizzo e numero di studenti.

Nel programma degli studenti la P1 richiede:

```cpp
std::vector<Aula> aule;
```

Ogni `Aula` contiene sigla, tipologia ed edificio.

La relazione con le tessere avviene **tramite sigla**:

```text
Tessera dell'orario classe
  materia = "Informatica"
  docente = "Rossi"
  aula    = "INF1"
                 |
                 | ricerca per sigla
                 v
ElencoAule
  [0] Aula("A101", "Aula", "Lotto 1")
  [1] Aula("INF1", "Laboratorio", "Lotto 2")
  [2] Aula("DADA03", "Aula DADA", "Lotto 3")
```

Non è necessario che `Tessera` contenga un puntatore a un oggetto `Aula`: la sigla permette di cercarlo quando serve, senza introdurre il problema dei puntatori invalidati dalla crescita del vettore.

### Un vincolo da distinguere

Nella fase di caricamento iniziale prepariamo un elenco con una voce per ogni sigla distinta effettivamente presente nell'orario. Se aggiungiamo manualmente un'aula all'archivio, questa potrebbe non essere ancora utilizzata in alcuna lezione.

Pertanto, **la corrispondenza esatta è verificata sui dati iniziali**, ma non è garantita automaticamente dopo ogni inserimento. Le regole di coerenza e il loro mantenimento sistematico saranno approfonditi nella P2.

## 11. Errori frequenti e come riconoscerli

| Errore | Perché è un problema | Correzione |
|---|---|---|
| `aule[0]` su vettore vuoto | Indice non valido | Usare `push_back` per inserire |
| Confondere `size()` con l'ultimo indice | Ultimo indice = `size()-1` solo se non vuoto | Usare `i < size()` |
| Inserire senza cercare | Possibili sigle duplicate | Controllare con `Cerca` |
| Dichiarare il vettore `public` | Il chiamante può aggirare i controlli | Attributo `private` |
| Usare `delete` sugli elementi di `vector<Aula>` | Non sono puntatori proprietari creati con `new` | Lasciare la gestione al vettore |
| Conservare `Get(i)` dopo `push_back` | Il puntatore può essere invalidato | Ripetere la ricerca |
| Restituire `Aula*` modificabile | Consente modifiche senza controlli | Preferire `const Aula*` |
| Mettere `cin` dentro `ElencoAule` | Mescola modello e interfaccia | Input in `InterfacciaConsole` |
| Sostituire la matrice dell'orario con un vettore | Cambia un componente fuori dagli obiettivi P1 | Conservare `Tessera* celle[5][8]` |

## 12. Esercizio svolto: un piccolo archivio indipendente

Supponiamo di usare `CatalogoLibri`:

```cpp
CatalogoLibri catalogo;

bool primo = catalogo.Aggiungi(Libro("L01", "Algoritmi"));
bool secondo = catalogo.Aggiungi(Libro("L02", "Reti"));
bool duplicato = catalogo.Aggiungi(Libro("L01", "Programmazione"));

int numero = catalogo.GetNumeroLibri();
int posizione = catalogo.Cerca("L02");
```

**Risultato atteso:**
- `primo == true`;
- `secondo == true`;
- `duplicato == false`, perché `L01` esiste già;
- `numero == 2`;
- `posizione == 1`.

La ricerca è per **codice**, non per titolo: due libri con titoli differenti ma con lo stesso codice sono considerati duplicati in questo semplice archivio.

## 13. Domande di autoverifica

1. Qual è la differenza tra un array di dimensione fissa e `std::vector`?
2. Che cosa restituisce `size()`? Che cosa indica `capacity()`?
3. Perché un vettore vuoto non permette di accedere all'elemento di indice zero?
4. Che cosa viene memorizzato in `std::vector<Libro>`: oggetti o puntatori?
5. Che cosa succede quando esegui `push_back(primo)`?
6. Perché non si devono eliminare con `delete` gli oggetti contenuti per valore nel vettore?
7. Come funziona una ricerca sequenziale? Qual è il risultato se non trova nulla?
8. Perché `Aggiungi` usa un parametro `const Libro&`?
9. Qual è la differenza tra `const Libro&` e `const Libro*`?
10. Perché `Get` deve controllare l'indice prima di restituire un indirizzo?
11. In quali circostanze `push_back` può invalidare i puntatori agli elementi?
12. Perché le tessere continuano a contenere la sigla dell'aula invece di un puntatore all'elemento del vettore?
13. Perché la ricerca e il controllo dei duplicati appartengono all'archivio e non alla console?

## 14. Esercizi di consolidamento

**Esercizio A — Prevedi il risultato.** Senza eseguire il programma, indica la dimensione del vettore dopo ciascuna istruzione:

```cpp
std::vector<int> numeri;
numeri.push_back(10);
numeri.push_back(20);
numeri.push_back(30);
```

Quali sono gli indici validi al termine? Qual è il valore di `numeri[1]`?

**Esercizio B — Individua l'errore.** Spiega perché il codice seguente non è corretto:

```cpp
std::vector<int> numeri;
numeri[0] = 10;
```

Riscrivilo usando l'operazione appropriata.

**Esercizio C — Progetta un archivio.** Definisci l'interfaccia di `ElencoLaboratori`, contenente `std::vector<Laboratorio>`, con metodi per inserire un laboratorio, cercarlo per codice e conoscere il numero di elementi. Non è necessario implementare subito i metodi.

**Esercizio D — Confronta le memorie.** Disegna uno schema con un oggetto `Libro` locale e una sua copia dentro `std::vector<Libro>`. Disegna poi una cella `Tessera*` che punta a una tessera creata con `new`. Indica in entrambi i casi chi è responsabile della distruzione degli oggetti.

**Esercizio E — Puntatore non proprietario.** Un programma esegue:

```cpp
const Libro* p = catalogo.Get(0);
catalogo.Aggiungi(Libro("L03", "Basi di dati"));
```

È sempre sicuro utilizzare `p` dopo l'inserimento? Motiva la risposta e indica una strategia più sicura.

---

## 15. Riepilogo finale

| Concetto | Da ricordare |
|---|---|
| `std::vector<T>` | Sequenza dinamica di elementi di tipo `T` |
| `push_back(x)` | Aggiunge un elemento in fondo |
| `size()` | Numero di elementi effettivamente presenti |
| `v[i]` | Accesso indicizzato senza controllo automatico dei limiti |
| Oggetti per valore | Il vettore contiene e gestisce gli oggetti, non soltanto i loro indirizzi |
| `const T&` | Accesso senza copia del parametro e senza modificarlo |
| Ricerca sequenziale | Esamina gli elementi uno per uno; nel caso peggiore O(n) |
| Incapsulamento | Il vettore è privato; i metodi pubblici controllano le operazioni |
| `const T*` restituito da `Get` | Puntatore di sola lettura, non proprietario, potenzialmente invalidabile |
| Orario P0 | Rimane una matrice di puntatori a tessere |
| Archivio P1 | È un vettore di oggetti per valore |

**Collegamento al laboratorio:** il docente applica questi concetti a `Classe` e `ElencoClassi`; gli studenti li applicano a `Aula` e `ElencoAule`. Nella P2 approfondiremo i vincoli di coerenza del modello e le operazioni che devono preservarli.
