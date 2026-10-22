namespace OrarioClasse;

public sealed class Orario
{
    public const int Giorni = 5;
    public const int Ore = 8;
    private readonly Tessera?[,] celle = new Tessera?[Giorni, Ore];

    public Tessera? Leggi(Posizione p)
    {
        Controlla(p);
        return celle[p.Giorno, p.Ora];
    }

    // Modifica interna: la GUI non può cambiare direttamente le celle.
    internal void Scrivi(Posizione p, Tessera? tessera)
    {
        Controlla(p);
        celle[p.Giorno, p.Ora] = tessera;
    }

    internal void Scambia(Posizione a, Posizione b)
    {
        (celle[a.Giorno, a.Ora], celle[b.Giorno, b.Ora]) =
            (celle[b.Giorno, b.Ora], celle[a.Giorno, a.Ora]);
    }

    public Orario Copia()
    {
        var copia = new Orario();
        for (int g = 0; g < Giorni; g++)
            for (int o = 0; o < Ore; o++)
                copia.celle[g, o] = celle[g, o];
        return copia;
    }

    private static void Controlla(Posizione p)
    {
        if (!p.Valida) throw new ArgumentOutOfRangeException(nameof(p));
    }

    public static Orario CreaEsempio()
    {
        var orario = new Orario();
        string[] materie = { "Matematica", "Italiano", "Informatica", "Inglese", "Storia", "Sistemi", "TPSIT", "Scienze", "Educazione fisica" };
        int indice = 0;
        for (int g = 0; g < Giorni; g++)
        {
            int oreDiLezione = g == 4 ? 6 : 7;
            for (int o = 0; o < oreDiLezione; o++)
            {
                orario.Scrivi(new Posizione(g, o),
                    new Tessera(materie[(indice * 3 + indice / 5) % materie.Length], $"Aula {(indice % 9) + 1}"));
                indice++;
            }
        }
        return orario;
    }
}
