# orario_scolastico
Progetto C# per la gestione dell'orario scolastico

ORARIOCLASSE - WPF .NET FRAMEWORK 4.8

Aprire OrarioClasse.sln in Visual Studio 2022 su Windows.
Installare il carico di lavoro Sviluppo di applicazioni desktop .NET e il targeting pack .NET Framework 4.8.
Selezionare OrarioClasse come progetto di avvio e premere F5.

Il codice didattico riprende le tre dispense fornite.
La differenza tecnica e' il file di progetto classico .csproj per .NET Framework 4.8.
Non serve Program.cs: il punto di ingresso WPF e' generato da App.xaml.

Prove:
- Selezionare due celle occupate e premere Scambia: autorizzato.
- Lunedì 7a ora e venerdì 7a ora vuota: autorizzato.
- Lunedì 3a ora e venerdì 7a ora vuota: rifiutato.

NOTA: la compilazione WPF richiede Visual Studio/MSBuild su Windows.