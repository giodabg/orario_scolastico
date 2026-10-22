using OrarioClasse;
namespace OrarioClasse;

public sealed class OrarioController
{
    private readonly MotoreScambi motore = new();
    public Orario Orario { get; } = Orario.CreaEsempio();
    public Posizione? Prima { get; private set; }
    public Posizione? Seconda { get; private set; }
    public bool Pronto => Prima.HasValue && Seconda.HasValue;

    public void Seleziona(Posizione posizione)
    {
        if (!posizione.Valida) return;
        if (Prima == posizione)
        {
            Prima = Seconda;
            Seconda = null;
        }
        else if (Seconda == posizione)
            Seconda = null;
        else if (Prima is null || Seconda is not null)
        {
            Prima = posizione;
            Seconda = null;
        }
        else Seconda = posizione;
    }

    public EsitoScambio RichiediScambio()
    {
        if (Prima is not { } a || Seconda is not { } b)
            return new(false, "Selezionare due celle.");
        var esito = motore.Scambia(Orario, a, b);
        Prima = null;
        Seconda = null;
        return esito;
    }
}
