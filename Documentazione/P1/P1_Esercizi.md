# P1 — Esercizi su std::vector e archivi di oggetti

**Quarta Informatica — versione studenti, 11 novembre 2026**  
**Prerequisiti:** P0, classi, array, riferimenti e puntatori.  
**Indicazioni:** per gli esercizi di codice usare C++17. Motivare le risposte; non utilizzare iteratori, algoritmi STL o smart pointer. Gli esercizi sono indipendenti dalla soluzione completa di `ElencoAule`.

## A — Comprensione e previsione

### E1 — Dimensione e indici (4 punti)
Considera:
```cpp
std::vector<int> valori;
valori.push_back(4);
valori.push_back(9);
valori.push_back(2);
```
Indica la dimensione dopo ciascun inserimento, gli indici validi alla fine e il valore di `valori[1]`.

### E2 — Individua l'errore (3 punti)
```cpp
std::vector<int> numeri;
numeri[0] = 12;
```
Spiega il problema e riscrivi la seconda istruzione.

### E3 — Prevedi l'output (4 punti)
```cpp
std::vector<int> numeri;
numeri.push_back(3);
numeri.push_back(6);
numeri.push_back(9);
for (int i = 0; i < static_cast<int>(numeri.size()); i++) {
    std::cout << numeri[i] * 2 << " ";
}
```
Scrivi esattamente i numeri stampati e il numero di iterazioni.

### E4 — Dimensione e capacità (3 punti)
Spiega la differenza tra `size()` e `capacity()`. Se `size() == 3` e `capacity() == 8`, è lecito leggere `v[5]`? Motiva.

## B — Oggetti, memoria e parametri

Per E5–E8 considera:
```cpp
class Libro {
private:
    std::string codice;
    std::string titolo;
public:
    Libro(std::string c, std::string t) : codice(c), titolo(t) {}
    std::string GetCodice() const { return codice; }
    std::string GetTitolo() const { return titolo; }
};
```

### E5 — Oggetti per valore (5 punti)
```cpp
Libro primo("L01", "Reti");
std::vector<Libro> libri;
libri.push_back(primo);
```
Disegna uno schema concettuale della memoria distinguendo l'oggetto `primo` e l'elemento `libri[0]`. Sono lo stesso oggetto? Chi distrugge l'elemento del vettore?

### E6 — Confronto con la P0 (5 punti)
Disegna e commenta le due situazioni:
```cpp
Tessera* p = new Tessera("Informatica", "Rossi", "INF1");
std::vector<Libro> libri;
libri.push_back(Libro("L02", "Algoritmi"));
```
Assumi la versione `Tessera` del progetto classe. Chi deve eseguire `delete p` se il puntatore non è stato trasferito a `Orario`? Serve `delete` per `libri[0]`?

### E7 — Scelta del parametro (4 punti)
Confronta `void Mostra(Libro libro)`, `void Mostra(Libro& libro)` e `void Mostra(const Libro& libro)`. Quale sceglieresti per una funzione che legge soltanto il libro, evitando una copia? Perché?

### E8 — Puntatore non proprietario (5 punti)
```cpp
const Libro* p = catalogo.Get(0);
catalogo.Aggiungi(Libro("L03", "Basi di dati"));
```
Supponi che `Get` restituisca l'indirizzo di un elemento interno di `std::vector<Libro>`. È garantito che `p` rimanga valido? È corretto eseguire `delete p`? Spiega come recuperare il libro in modo sicuro dopo l'aggiunta.

## C — Algoritmi e incapsulamento

### E9 — Completa la ricerca (6 punti)
Completa i tre spazi senza usare algoritmi STL:
```cpp
int Cerca(const std::vector<Libro>& libri, std::string codice) {
    for (int i = 0; i < __________; i++) {
        if (________________________ == codice) {
            return __________;
        }
    }
    return -1;
}
```
Spiega perché si usa `-1` per l'assenza.

### E10 — Ricerca sequenziale (4 punti)
Un vettore contiene, nell'ordine, i codici `L01`, `L05`, `L09`, `L12`. Quanti confronti sono necessari per cercare `L09`? E per cercare `L99`? Indica il costo nel caso peggiore rispetto al numero `n` di elementi.

### E11 — Inserimento controllato (6 punti)
Scrivi il corpo di `bool CatalogoLibri::Aggiungi(const Libro& libro)`, sapendo che la classe possiede `std::vector<Libro> libri` e `int Cerca(std::string codice) const`. Rifiuta codici vuoti e duplicati, inserisci con `push_back`, restituisci l'esito.

### E12 — Accesso sicuro (6 punti)
Scrivi il corpo di `const Libro* CatalogoLibri::Get(int indice) const`, con `int GetNumeroLibri() const`. Per indici non validi restituisci `nullptr`. Spiega perché il tipo restituito è `const Libro*`.

## D — Applicazione autonoma

### E13 — Progettazione (8 punti)
Progetta soltanto l'header `ElencoLaboratori.h` per un archivio di oggetti `Laboratorio` (già definito), identificati da un codice. Il vettore deve essere privato. Prevedi `Aggiungi`, `Cerca`, `GetNumeroLaboratori`, `Get` con le stesse regole studiate.

### E14 — Correggi il codice (7 punti)
Individua **almeno quattro** errori concettuali o tecnici:
```cpp
class Archivio {
public:
    std::vector<Libro> libri;

    bool Aggiungi(const Libro& libro) {
        libri.push_back(libro);
        return true;
    }

    Libro* Get(int i) {
        return &libri[i];
    }
};
```
Proponi le modifiche per proteggere l'archivio da duplicati, accessi non validi e modifiche incontrollate.

### E15 — Trasferimento al progetto orario (10 punti)
Senza scrivere tutta l'applicazione, descrivi: (a) quali attributi contiene `Aula`; (b) quali metodi offre `ElencoAule`; (c) come `DatiEsempio` carica le aule; (d) perché `Tessera` continua a contenere la sigla dell'aula e non un puntatore all'elemento del vettore; (e) quali tre opzioni aggiungere al menu.

---

**Totale: 80 punti.** Per le prove di programmazione è sufficiente il codice richiesto, non l'intero progetto. Il codice degli esempi presuppone gli `#include` necessari e, per E6, i costruttori già previsti nella P0.
