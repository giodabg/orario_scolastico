# P1 — Soluzioni commentate degli esercizi

**Versione docente — 11 novembre 2026**  
**Documento associato:** `P1_Esercizi.md` — 15 esercizi, massimo 80 punti.  
**Uso:** correzione e discussione; non distribuire insieme alla traccia.

## E1 (4 punti)
Dimensioni successive: **1, 2, 3**. Indici validi: **0, 1, 2**. `valori[1] == 9`. Gli indici partono da zero.

## E2 (3 punti)
`numeri` è vuoto; `numeri[0]` accede a un elemento inesistente, con comportamento indefinito. Correzione: `numeri.push_back(12);`.

## E3 (4 punti)
Output: `6 12 18 ` (uno spazio dopo ogni numero). Iterazioni: **3**. Il ciclo attraversa gli indici 0, 1 e 2.

## E4 (3 punti)
`size()` conta gli elementi effettivamente presenti; `capacity()` indica lo spazio riservato per gli elementi prima di un'eventuale riallocazione. **No**, `v[5]` è fuori dai limiti perché 5 non è minore di 3. La capacità non rende accessibili elementi non costruiti.

## E5 (5 punti)
Schema:
```text
primo (oggetto locale)          libri (vector<Libro>)
+----------------------+        +----------------------+
| L01 / Reti           |        | [0] L01 / Reti       |
+----------------------+        +----------------------+
             copia con push_back ----^
```
Sono **due oggetti distinti**. Il vettore distrugge il proprio elemento quando viene distrutto (o quando l'elemento viene rimosso). Non serve `delete`.

## E6 (5 punti)
```text
p (puntatore locale) ----> Tessera allocata con new
libri (vector<Libro>) --> [ Libro("L02", "Algoritmi") ]
```
Se `p` non viene passato con successo a `Orario::Set` o a un altro proprietario, il chiamante deve eseguire `delete p;`. Se `Set` lo acquisisce, sarà `Orario` a distruggerlo. L'elemento di `vector<Libro>` è gestito dal contenitore; **non** si usa `delete`.

## E7 (4 punti)
`Libro libro`: copia l'oggetto. `Libro& libro`: non copia ma permette modifiche. `const Libro& libro`: non copia e non permette modifiche attraverso il parametro. Scelta: **`const Libro&`**.

## E8 (5 punti)
**No**, `push_back` può causare una riallocazione e invalidare `p`. **No**, il puntatore è non proprietario e non va eliminato con `delete` (inoltre punta a `const`). Dopo l'aggiunta, cercare nuovamente il codice e ottenere un nuovo puntatore con `Get`; controllare `nullptr` prima dell'uso.

## E9 (6 punti)
```cpp
int Cerca(const std::vector<Libro>& libri, std::string codice) {
    for (int i = 0; i < static_cast<int>(libri.size()); i++) {
        if (libri[i].GetCodice() == codice) {
            return i;
        }
    }
    return -1;
}
```
`-1` è un valore sentinella che non coincide con alcun indice valido. Assegnare 2 punti per ciascuno dei tre spazi corretti.

## E10 (4 punti)
`L09`: **3** confronti. `L99`: **4** confronti. Nel caso peggiore la ricerca sequenziale richiede **O(n)** confronti.

## E11 (6 punti)
```cpp
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
```
Criteri: codice vuoto (1), duplicato (2), inserimento (2), esito (1). `push_back` memorizza una copia; non serve `new`.

## E12 (6 punti)
```cpp
const Libro* CatalogoLibri::Get(int indice) const {
    if (indice < 0 || indice >= GetNumeroLibri()) {
        return nullptr;
    }
    return &libri[indice];
}
```
Criteri: indice negativo (1), limite superiore (2), `nullptr` (1), indirizzo elemento (1), motivazione `const` (1). Il puntatore non consente di modificare l'elemento tramite l'accesso restituito; rimane non proprietario e può essere invalidato da `push_back`.

## E13 (8 punti)
```cpp
#pragma once
#include <vector>
#include <string>
#include "Laboratorio.h"

class ElencoLaboratori {
private:
    std::vector<Laboratorio> laboratori;

public:
    bool Aggiungi(const Laboratorio& laboratorio);
    int Cerca(std::string codice) const;
    int GetNumeroLaboratori() const;
    const Laboratorio* Get(int indice) const;
};
```
Valutare: header/inclusioni (1), vettore privato per valore (2), quattro firme (4), corretto uso di `const` (1).

## E14 (7 punti)
Problemi e correzioni:
1. `libri` è `public`: renderlo `private`.
2. `Aggiungi` non controlla codice vuoto: rifiutarlo.
3. `Aggiungi` non verifica duplicati: usare una ricerca per codice.
4. `Get` non controlla `i < 0` e `i >= size()`: restituire `nullptr` se non valido.
5. `Get` restituisce `Libro*` modificabile: preferire `const Libro*` e dichiarare il metodo `const`.
6. La classe non espone una ricerca o un conteggio controllati: aggiungere `Cerca` e `GetNumeroLibri` secondo il contratto dell'archivio.

Assegnare fino a 7 punti per quattro problemi ben motivati e modifiche coerenti. Non è necessario fornire una classe completa per ottenere il massimo.

## E15 (10 punti)
(a) `Aula`: `sigla`, `tipologia`, `edificio` privati, costruttore e metodi `Get` (2).  
(b) `ElencoAule`: `std::vector<Aula>` privato; `Aggiungi(const Aula&)`, `Cerca(std::string) const`, `GetNumeroAule() const`, `Get(int) const` con risultato `const Aula*` (2).  
(c) `DatiEsempio` inserisce un oggetto per ogni sigla distinta presente nell'orario iniziale, controllando gli esiti (2).  
(d) La sigla evita puntatori a elementi del vettore che possono essere invalidati dalla crescita; l'associazione si ottiene tramite ricerca (2).  
(e) Menu: visualizza aule, cerca aula, aggiungi aula, preservando le opzioni P0 (2).

## Griglia di valutazione

| Sezione | Esercizi | Punti |
|---|---|---:|
| A — Comprensione | E1–E4 | 14 |
| B — Memoria e parametri | E5–E8 | 19 |
| C — Algoritmi e incapsulamento | E9–E12 | 22 |
| D — Applicazione | E13–E15 | 25 |
| **Totale** | **15 esercizi** | **80** |

**Nota docente:** accettare soluzioni equivalenti e corrette (ad esempio confronti logicamente equivalenti). Penalizzare in modo specifico l'accesso fuori dai limiti, la restituzione di puntatori modificabili quando contraria al contratto, e l'uso di `delete` su elementi posseduti dal vettore.
