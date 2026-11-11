# OrarioDocenteCPP — P0 e P1

Prototipo console C++17 dell'orario di **un solo docente**. Ogni tessera contiene materia, classe e aula. La matrice `Tessera* celle[5][8]` e' volutamente conservata dal prototipo precedente.

## Ambiente di sviluppo

Il percorso didattico utilizza **Visual Studio** con un normale progetto **Applicazione console C++**. Aggiungere al progetto i file `.h` e `.cpp` delle cartelle `Model`, `Engine` e della directory principale, quindi compilare ed eseguire dall'IDE.

Non sono richiesti generatori di progetto o strumenti di build esterni. I file `.h` non vanno compilati separatamente.

## Responsabilita'

- `Model/Classe`: anagrafica di una classe (P1).
- `Model/ElencoClassi`: archivio dinamico `std::vector<Classe>` (P1).
- `Model/Tessera`: dati della lezione (materia, classe, aula).
- `Model/Posizione`: indici giorno/ora, da zero.
- `Model/Orario`: proprietario delle tessere, accesso e scambio delle celle.
- `Engine/ValidatoreOrario`: controlla l'assenza di ore vuote prima di lezioni successive.
- `Engine/MotoreScambi`: scambia e ripristina in caso di fallimento.
- `DatiEsempio`: costruisce l'orario e l'elenco iniziale delle classi.
- `InterfacciaConsole`: menu, input e visualizzazione.

## Limiti voluti

Il programma non gestisce conflitti tra docenti/classi, salvataggio su file o generazione automatica. Lo scambio e' un prototipo per verificare il comportamento delle classi. `Get` restituisce un puntatore non proprietario modificabile: non conservarlo oltre la durata dell'orario, non eliminarlo e non usarlo per alterare gli invarianti. La copia di `Orario` e' disabilitata per evitare doppie deallocazioni.

`Set` accetta soltanto celle vuote e acquisisce la proprieta' del puntatore solo se restituisce `true`. In caso contrario il chiamante deve eliminare la tessera non inserita.
