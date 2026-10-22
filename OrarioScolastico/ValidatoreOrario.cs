using OrarioClasse;
namespace OrarioClasse;

public sealed class ValidatoreOrario
{
    public bool NessunBuco(Orario orario)
    {
        for (int g = 0; g < Orario.Giorni; g++)
        {
            bool trovataCellaVuota = false;
            for (int o = 0; o < Orario.Ore; o++)
            {
                bool occupata = orario.Leggi(new Posizione(g, o)) is not null;
                if (!occupata) trovataCellaVuota = true;
                else if (trovataCellaVuota) return false;
            }
        }
        return true;
    }
}
