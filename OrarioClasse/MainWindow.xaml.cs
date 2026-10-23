using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrarioClasse.Model;
using OrarioClasse.Engine;

namespace OrarioClasse
{
    public partial class MainWindow : Window
    {
        private Orario orario;
        private MotoreScambi motore;
        private Button[,] pulsanti;
        private Posizione primaSelezione;
        private Posizione secondaSelezione;

        public MainWindow()
        {
            InitializeComponent();
            orario = new Orario();
            motore = new MotoreScambi();
            pulsanti = new Button[5, 8];
            CaricaOrarioEsempio();
            CreaScacchiera();
            AggiornaScacchiera();
            AggiornaSelezione();
        }

        // Crea 34 tessere senza buchi: 7+7+7+7+6.
        private void CaricaOrarioEsempio()
        {
            string[] materie = {
                "Matematica", "Informatica", "Italiano",
                "Inglese", "Sistemi", "TPS", "Storia"
            };
            for (int giorno = 0; giorno < 5; giorno++)
            {
                int lezioni = 7;
                if (giorno == 4)
                    lezioni = 6;
                for (int ora = 0; ora < lezioni; ora++)
                {
                    string materia = materie[(giorno + ora) % materie.Length];
                    string aula = "A" + (ora + 1);
                    Tessera tessera = new Tessera(materia, aula);
                    orario.Scrivi(new Posizione(giorno, ora), tessera);
                }
            }
        }

        private void CreaScacchiera()
        {
            string[] giorni = { "Lun", "Mar", "Mer", "Gio", "Ven" };
            // Una colonna per le ore, cinque per i giorni.
            for (int colonna = 0; colonna < 6; colonna++)
                grigliaOrario.ColumnDefinitions.Add(new ColumnDefinition());
            // Una riga per le intestazioni, otto per le ore.
            for (int riga = 0; riga < 9; riga++)
                grigliaOrario.RowDefinitions.Add(new RowDefinition());

            for (int giorno = 0; giorno < 5; giorno++)
            {
                TextBlock intestazione = new TextBlock();
                intestazione.Text = giorni[giorno];
                intestazione.FontWeight = FontWeights.Bold;
                intestazione.HorizontalAlignment = HorizontalAlignment.Center;
                intestazione.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(intestazione, 0);
                Grid.SetColumn(intestazione, giorno + 1);
                grigliaOrario.Children.Add(intestazione);
            }

            for (int ora = 0; ora < 8; ora++)
            {
                TextBlock numeroOra = new TextBlock();
                numeroOra.Text = (ora + 1) + "ª";
                numeroOra.HorizontalAlignment = HorizontalAlignment.Center;
                numeroOra.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(numeroOra, ora + 1);
                Grid.SetColumn(numeroOra, 0);
                grigliaOrario.Children.Add(numeroOra);
            }

            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    Button pulsante = new Button();
                    pulsante.Margin = new Thickness(3);
                    pulsante.Tag = new Posizione(giorno, ora);
                    pulsante.Click += Cella_Click;
                    Grid.SetRow(pulsante, ora + 1);
                    Grid.SetColumn(pulsante, giorno + 1);
                    grigliaOrario.Children.Add(pulsante);
                    pulsanti[giorno, ora] = pulsante;
                }
            }
        }

        private void AggiornaScacchiera()
        {
            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    Tessera tessera = orario.Leggi(new Posizione(giorno, ora));
                    Button pulsante = pulsanti[giorno, ora];
                    if (tessera == null)
                    {
                        pulsante.Content = "—";
                    }
                    else
                    {
                        StackPanel contenuto = new StackPanel();
                        TextBlock materia = new TextBlock();
                        materia.Text = tessera.Materia;
                        materia.FontWeight = FontWeights.Bold;
                        materia.TextAlignment = TextAlignment.Center;
                        TextBlock aula = new TextBlock();
                        aula.Text = tessera.Aula;
                        aula.TextAlignment = TextAlignment.Center;
                        contenuto.Children.Add(materia);
                        contenuto.Children.Add(aula);
                        pulsante.Content = contenuto;
                    }
                }
            }
        }

        private bool StessaPosizione(Posizione a, Posizione b)
        {
            if (a == null || b == null)
                return false;
            return a.Giorno == b.Giorno && a.Ora == b.Ora;
        }

        private void Cella_Click(object sender, RoutedEventArgs e)
        {
            Button pulsante = (Button)sender;
            Posizione scelta = (Posizione)pulsante.Tag;
            if (StessaPosizione(scelta, primaSelezione))
            {
                primaSelezione = secondaSelezione;
                secondaSelezione = null;
            }
            else if (StessaPosizione(scelta, secondaSelezione))
            {
                secondaSelezione = null;
            }
            else if (primaSelezione == null || secondaSelezione != null)
            {
                primaSelezione = scelta;
                secondaSelezione = null;
            }
            else
            {
                secondaSelezione = scelta;
            }
            txtStato.Text = "Seleziona due celle e premi Scambia.";
            AggiornaSelezione();
        }

        private void AggiornaSelezione()
        {
            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    Posizione posizione = new Posizione(giorno, ora);
                    Button pulsante = pulsanti[giorno, ora];
                    if (StessaPosizione(posizione, primaSelezione) ||
                        StessaPosizione(posizione, secondaSelezione))
                        pulsante.Background = Brushes.LightSkyBlue;
                    else
                        pulsante.Background = Brushes.WhiteSmoke;
                }
            }
            btnScambia.IsEnabled =
                primaSelezione != null && secondaSelezione != null;
        }

        private void Scambia_Click(object sender, RoutedEventArgs e)
        {
            if (primaSelezione == null || secondaSelezione == null)
                return;
            bool riuscito = motore.Scambia(orario, primaSelezione, secondaSelezione);
            if (riuscito)
                txtStato.Text = "Scambio autorizzato.";
            else
                txtStato.Text = "Scambio rifiutato: si creerebbe un buco.";
            primaSelezione = null;
            secondaSelezione = null;
            AggiornaScacchiera();
            AggiornaSelezione();
        }
    }
}
