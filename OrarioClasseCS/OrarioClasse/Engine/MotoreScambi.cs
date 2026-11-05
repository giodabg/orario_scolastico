using OrarioClasse.Model;

namespace OrarioClasse.Engine
{
    public class MotoreScambi
    {
        private ValidatoreOrario validatore;

        public MotoreScambi()
        {
            validatore = new ValidatoreOrario();
        }

        public bool Scambia(Orario orario, Posizione prima, Posizione seconda)
        {
            // Conserva i contenuti originali.
            Tessera tessera1 = orario.Leggi(prima);
            Tessera tessera2 = orario.Leggi(seconda);

            // Esegue provvisoriamente lo scambio.
            orario.Scrivi(prima, tessera2);
            orario.Scrivi(seconda, tessera1);

            // Se il nuovo orario è valido, conferma lo scambio.
            if (validatore.SenzaBuchi(orario))
            {
                return true;
            }

            // Altrimenti ripristina la situazione precedente.
            orario.Scrivi(prima, tessera1);
            orario.Scrivi(seconda, tessera2);
            return false;
        }
    }
}
