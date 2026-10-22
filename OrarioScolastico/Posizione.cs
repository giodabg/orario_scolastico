namespace OrarioClasse;

// Giorno: 0=lunedì ... 4=venerdì; Ora: 0=prima ... 7=ottava.
public readonly record struct Posizione(int Giorno, int Ora)
{
    public bool Valida => Giorno is >= 0 and < 5 && Ora is >= 0 and < 8;
}
