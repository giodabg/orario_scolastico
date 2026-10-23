namespace OrarioClasse.Model
{
    public class Orario
    {
        private Tessera[,] celle;

        public Orario()
        {
            celle = new Tessera[5, 8];
        }

        public Tessera Leggi(Posizione posizione)
        {
            return celle[posizione.Giorno, posizione.Ora];
        }

        public void Scrivi(Posizione posizione, Tessera tessera)
        {
            celle[posizione.Giorno, posizione.Ora] = tessera;
        }
    }
}
