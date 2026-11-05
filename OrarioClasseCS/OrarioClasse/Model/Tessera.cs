namespace OrarioClasse.Model
{
    public class Tessera
    {
        public string Materia { get; set; }
        public string Aula { get; set; }

        public Tessera(string materia, string aula)
        {
            Materia = materia;
            Aula = aula;
        }
    }
}
