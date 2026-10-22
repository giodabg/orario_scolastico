# OrarioClasse — Prototipo 1

Applicazione desktop Windows in C# / WPF (.NET 8).

## Avvio
1. Installare Visual Studio 2022 (17.8 o successivo) con il workload **Sviluppo di applicazioni desktop .NET**.
2. Aprire `OrarioClasse.csproj` in Visual Studio.
3. Avviare con F5. In alternativa, su Windows con .NET 8 SDK: `dotnet run` nella cartella del progetto.

## Utilizzo
- Selezionare una cella e poi una seconda cella (anche vuota).
- Premere **Scambia**.
- Il motore simula lo scambio su una copia dell'orario e lo applica solo se non si creano buchi.
- Per cambiare selezione, fare clic su una cella già selezionata oppure, dopo due selezioni, su una nuova cella.

## Struttura
- `Model`: tessera (materia, aula), posizione e matrice dell'orario.
- `Engine`: motore degli scambi, validatore e risultato.
- `Controller`: stato delle due selezioni e richiesta al motore.
- `View`: XAML e creazione dei 40 pulsanti nel code-behind; nessuna regola didattica nella GUI.

## Dati di esempio
34 lezioni: 7 ore dal lunedì al giovedì e 6 il venerdì. Le altre 6 celle sono vuote, tutte a fine giornata. Le materie e le aule sono dati dimostrativi.

## Limitazioni intenzionali
Nessun salvataggio, database, controllo delle disponibilità dei docenti o delle aule. Il solo vincolo è l'assenza di buchi. Le celle vuote sono selezionabili; due celle vuote non possono essere scambiate.
