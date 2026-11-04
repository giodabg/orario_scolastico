# OrarioDocenteCPP — P0

Prototipo console C++17 dell'orario di **un solo docente**. Ogni tessera contiene materia, classe e aula. La matrice `Tessera* celle[5][8]` e' volutamente conservata dal prototipo precedente.

## Compilazione

Con g++ (dalla cartella `OrarioDocenteCPP`):

```bash
g++ -std=c++17 -Wall -Wextra -pedantic main.cpp Model/*.cpp Engine/*.cpp DatiEsempio.cpp InterfacciaConsole.cpp -o OrarioDocente
./OrarioDocente
```

In alternativa, aprire la cartella come progetto CMake in Visual Studio.

## Responsabilita'

- `Model/Tessera`: dati della lezione (materia, classe, aula).
- `Model/Posizione`: indici giorno/ora, da zero.
- `Model/Orario`: proprietario delle tessere, accesso e scambio delle celle.
- `Engine/ValidatoreOrario`: controlla l'assenza di ore vuote prima di lezioni successive.
- `Engine/MotoreScambi`: scambia e ripristina in caso di fallimento.
- `DatiEsempio`: costruisce l'orario iniziale.
- `InterfacciaConsole`: menu, input e visualizzazione.

## Limiti voluti

Il programma non gestisce conflitti tra docenti/classi, salvataggio su file o generazione automatica. Lo scambio e' un prototipo per verificare il comportamento delle classi. `Get` restituisce un puntatore non proprietario modificabile: non conservarlo oltre la durata dell'orario, non eliminarlo e non usarlo per alterare gli invarianti. La copia di `Orario` e' disabilitata per evitare doppie deallocazioni.

`Set` accetta soltanto celle vuote e acquisisce la proprieta' del puntatore solo se restituisce `true`. In caso contrario il chiamante deve eliminare la tessera non inserita.
