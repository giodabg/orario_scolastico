using OrarioClasse.Model;

namespace OrarioClasse.Engine
{
    public class ValidatoreOrario
    {
        public bool SenzaBuchi(Orario orario)
        {
            for (int giorno = 0; giorno < 5; giorno++)
            {
                bool trovataCellaVuota = false;
                for (int ora = 0; ora < 8; ora++)
                {
                    Posizione posizione = new Posizione(giorno, ora);
                    Tessera tessera = orario.Leggi(posizione);
                    if (tessera == null)
                    {
                        trovataCellaVuota = true;
                    }
                    else if (trovataCellaVuota)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
