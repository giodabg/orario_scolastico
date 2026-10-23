namespace OrarioClasse.Model
{
    public class Posizione
    {
        public int Giorno { get; set; }
        public int Ora { get; set; }

        public Posizione(int giorno, int ora)
        {
            Giorno = giorno;
            Ora = ora;
        }
    }
}
