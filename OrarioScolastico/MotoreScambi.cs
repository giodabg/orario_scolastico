using OrarioClasse;
namespace OrarioClasse;

public sealed class MotoreScambi
{
    private readonly ValidatoreOrario validatore = new();

    public EsitoScambio Scambia(Orario orario, Posizione prima, Posizione seconda)
    {
        if (!prima.Valida || !seconda.Valida)
            return new(false, "Posizione non valida.");
        if (prima == seconda)
            return new(false, "Selezionare due celle diverse.");
        if (orario.Leggi(prima) is null && orario.Leggi(seconda) is null)
            return new(false, "Entrambe le celle sono vuote.");
        if (!validatore.NessunBuco(orario))
            return new(false, "L'orario iniziale contiene già un buco.");

        var prova = orario.Copia();
        prova.Scambia(prima, seconda);
        if (!validatore.NessunBuco(prova))
            return new(false, "Scambio rifiutato: creerebbe un buco nell'orario.");

        orario.Scambia(prima, seconda);
        return new(true, "Scambio autorizzato ed eseguito.");
    }
}
