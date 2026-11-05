using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private ImageBrush sfondoOraVuota;

        public MainWindow()
        {
            InitializeComponent();
            orario = new Orario();
            motore = new MotoreScambi();
            pulsanti = new Button[5, 8];

            // Costruisce il percorso della risorsa grafica concatenando
            // la cartella Immagini con il nome del file PNG.
            // Il prefisso "pack://application:,,,/" indica che l'immagine
            // è una risorsa incorporata nell'applicazione WPF.
            // Affinché il caricamento funzioni, il file PNG deve essere
            // incluso nel progetto Visual Studio con la proprietà
            // Build Action/Azione di compilazione = Resource
            sfondoOraVuota = new ImageBrush(
                new BitmapImage(new Uri("pack://application:,,,/Immagini/vuota.png", UriKind.Absolute)));
            sfondoOraVuota.Stretch = Stretch.Fill;

            CaricaOrarioEsempio();
            CreaScacchiera();
            AggiornaScacchiera();
            AggiornaSelezione();
        }

        // Crea tessere con orario ufficiale.
        private void CaricaOrarioEsempio()
        {
            // Giorni: 0=Lunedì, 1=Martedì, 2=Mercoledì,
            //         3=Giovedì, 4=Venerdì
            // Ore:    0=8:00, 1=9:00, 2=10:00,
            //         3=11:00, 4=12:00, 5=13:00,
            //         6=14:10, 7=15:00

            // LUNEDÌ
            orario.Scrivi(new Posizione(0, 0),
                new Tessera("Telecomunicazioni", ""));

            orario.Scrivi(new Posizione(0, 1),
                new Tessera("Italiano", ""));

            orario.Scrivi(new Posizione(0, 2),
                new Tessera("Italiano", ""));

            orario.Scrivi(new Posizione(0, 3),
                new Tessera("Informatica", ""));

            orario.Scrivi(new Posizione(0, 4),
                new Tessera("Sistemi", "L3INF8"));

            orario.Scrivi(new Posizione(0, 5),
                new Tessera("Sistemi", "L3INF8"));


            // MARTEDÌ
            orario.Scrivi(new Posizione(1, 0),
                new Tessera("Religione", ""));

            orario.Scrivi(new Posizione(1, 1),
                new Tessera("Matematica", ""));

            orario.Scrivi(new Posizione(1, 2),
                new Tessera("Matematica", ""));

            orario.Scrivi(new Posizione(1, 3),
                new Tessera("Inglese", ""));

            orario.Scrivi(new Posizione(1, 4),
                new Tessera("Storia", ""));

            orario.Scrivi(new Posizione(1, 5),
                new Tessera("TPS", ""));

            orario.Scrivi(new Posizione(1, 6),
                new Tessera("Informatica", "L3INF7"));

            orario.Scrivi(new Posizione(1, 7),
                new Tessera("Italiano", ""));


            // MERCOLEDÌ
            orario.Scrivi(new Posizione(2, 0),
                new Tessera("Scienze motorie", ""));

            orario.Scrivi(new Posizione(2, 1),
                new Tessera("Scienze motorie", "Palestra"));

            orario.Scrivi(new Posizione(2, 2),
                new Tessera("Telecomunicazioni", "L3TEL"));

            orario.Scrivi(new Posizione(2, 3),
                new Tessera("Telecomunicazioni", "L3TEL"));

            orario.Scrivi(new Posizione(2, 4),
                new Tessera("Sistemi", ""));

            orario.Scrivi(new Posizione(2, 5),
                new Tessera("Matematica", ""));


            // GIOVEDÌ
            orario.Scrivi(new Posizione(3, 0),
                new Tessera("TPS", "L3INF7"));

            orario.Scrivi(new Posizione(3, 1),
                new Tessera("TPS", "L3INF7"));

            orario.Scrivi(new Posizione(3, 2),
                new Tessera("Informatica", ""));

            orario.Scrivi(new Posizione(3, 3),
                new Tessera("Informatica", ""));

            orario.Scrivi(new Posizione(3, 4),
                new Tessera("Inglese", ""));

            orario.Scrivi(new Posizione(3, 5),
                new Tessera("Italiano", ""));


            // VENERDÌ
            orario.Scrivi(new Posizione(4, 0),
                new Tessera("Sistemi", ""));

            orario.Scrivi(new Posizione(4, 1),
                new Tessera("Informatica", "L3INF7"));

            orario.Scrivi(new Posizione(4, 2),
                new Tessera("Informatica", "L3INF7"));

            orario.Scrivi(new Posizione(4, 3),
                new Tessera("Inglese", ""));

            orario.Scrivi(new Posizione(4, 4),
                new Tessera("Storia", ""));

            orario.Scrivi(new Posizione(4, 5),
                new Tessera("Matematica", ""));
        }


        private void CreaScacchiera()
        {
            string[] giorni = { "Lun", "Mar", "Mer", "Gio", "Ven" };
            // Crea le 6 colonne della griglia:
            // - la prima (indice 0) contiene i numeri delle ore;
            // - le altre 5 (indici da 1 a 5) contengono i giorni.
            for (int colonna = 0; colonna < 6; colonna++)
                grigliaOrario.ColumnDefinitions.Add(new ColumnDefinition());

            // Crea le 9 righe della griglia:
            // - la prima (indice 0) contiene le intestazioni dei giorni;
            // - le altre 8 (indici da 1 a 8) contengono le ore.
            for (int riga = 0; riga < 9; riga++)
                grigliaOrario.RowDefinitions.Add(new RowDefinition());

            // Inserisce nella prima riga le intestazioni dei 5 giorni.
            // A ogni iterazione crea un TextBlock e lo colloca
            // nella colonna corrispondente al giorno.
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

            // Inserisce nella prima colonna i numeri delle 8 ore.
            // A ogni iterazione crea un TextBlock con il numero
            // progressivo dell'ora (1ª, 2ª, ..., 8ª).
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

            // Crea i 40 pulsanti che rappresentano le celle
            // dell'orario (5 giorni per 8 ore).
            // Il ciclo esterno scorre i giorni, quello interno
            // scorre le 8 ore di ciascun giorno.
            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    Button pulsante = new Button();

                    // Imposta un margine di 3 pixel su tutti i lati del pulsante.
                    pulsante.Margin = new Thickness(3);
                    
                    // Memorizza nella proprietà Tag il riferimento all'oggetto che contiene
                    // la posizione (giorno, ora) della cella dell'orario associata al pulsante.
                    pulsante.Tag = new Posizione(giorno, ora);
                    
                    // Associa l'evento Click al metodo Cella_Click,
                    // che WPF esegue quando l'utente preme il pulsante.
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
            // Scorre tutte le 40 posizioni dell'orario:
            // il ciclo esterno seleziona uno dei 5 giorni,
            // quello interno una delle 8 ore del giorno.
            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    // Recupera dal modello la tessera presente
                    // nella posizione indicata da giorno e ora.
                    // Se la posizione è vuota, Leggi restituisce null.
                    Tessera tessera = orario.Leggi(new Posizione(giorno, ora));

                    // Recupera il pulsante WPF associato alla stessa
                    // posizione, precedentemente creato e memorizzato
                    // nella matrice pulsanti.
                    Button pulsante = pulsanti[giorno, ora];
                    if (tessera == null)
                    {
                        // Se non è presente alcuna lezione,
                        // elimina il contenuto del pulsante.
                        // L'immagine viene visualizzata tramite Background.
                        pulsante.Content = null;
                    }
                    else
                    {
                        // Crea un contenitore StackPanel che dispone
                        // i controlli figli uno sotto l'altro.
                        // Sarà utilizzato come contenuto del pulsante.
                        StackPanel contenuto = new StackPanel();

                        // PRIMO ELEMENTO: ICONA DELLA MATERIA
                        Image icona = new Image();
                        icona.Source = ImmagineMateria(tessera.Materia);
                        icona.Width = 32;
                        icona.Height = 32;
                        icona.Stretch = Stretch.Uniform;
                        icona.HorizontalAlignment = HorizontalAlignment.Center;
                        // Aggiunge l'immagine allo StackPanel.
                        contenuto.Children.Add(icona);

                        // SECONDO ELEMENTO: NOME DELLA MATERIA
                        TextBlock materia = new TextBlock();
                        materia.Text = tessera.Materia;
                        materia.FontWeight = FontWeights.Bold;
                        materia.TextAlignment = TextAlignment.Center;
                        // Aggiunge il TextBlock allo StackPanel sotto l'immagine.
                        contenuto.Children.Add(materia);

                        // TERZO ELEMENTO: AULA
                        TextBlock aula = new TextBlock();
                        aula.Text = tessera.Aula;
                        aula.TextAlignment = TextAlignment.Center;
                        // Aggiunge il TextBlock dello StackPanel sotto i due elementi precedenti.
                        contenuto.Children.Add(aula);

                        // Assegna lo StackPanel alla proprietà Content
                        // del pulsante, sostituendo il contenuto
                        // precedentemente visualizzato.
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
            // Converte il riferimento all'oggetto che ha generato l'evento
            // (sender) in un riferimento di tipo Button.
            Button pulsante = (Button)sender;

            // Recupera dalla proprietà Tag il riferimento all'oggetto
            // Posizione associato al pulsante e lo assegna a scelta.
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
            // Scorre tutte le 40 posizioni dell'orario:
            // il ciclo esterno seleziona uno dei 5 giorni,
            // quello interno una delle 8 ore del giorno.
            for (int giorno = 0; giorno < 5; giorno++)
            {
                for (int ora = 0; ora < 8; ora++)
                {
                    Posizione posizione = new Posizione(giorno, ora);
                    Button pulsante = pulsanti[giorno, ora];

                    // Controlla se la posizione corrente coincide
                    // con una delle due posizioni selezionate.
                    // In caso affermativo, assegna al pulsante
                    // uno sfondo azzurro per evidenziarlo.
                    if (StessaPosizione(posizione, primaSelezione) ||
                        StessaPosizione(posizione, secondaSelezione))
                        pulsante.Background = Brushes.LightSkyBlue;

                    // Se la posizione non è selezionata e non
                    // contiene una tessera, assegna al pulsante
                    // l'immagine di sfondo delle ore vuote.
                    else if (orario.Leggi(posizione) == null)
                        pulsante.Background = sfondoOraVuota;

                    // Se la posizione non è selezionata e contiene
                    // una tessera, assegna lo sfondo grigio chiaro.
                    else
                        pulsante.Background = Brushes.WhiteSmoke;
                }
            }

            // Abilita il pulsante Scambia soltanto quando
            // entrambe le posizioni sono state selezionate.
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

        private ImageSource ImmagineMateria(string materia)
        {
            string file;

            switch (materia)
            {
                case "Matematica": file = "matematica.png"; break;
                case "Informatica": file = "informatica.png"; break;
                case "Italiano": file = "italiano.png"; break;
                case "Inglese": file = "inglese.png"; break;
                case "Sistemi": file = "sistemi.png"; break;
                case "TPS": file = "tps.png"; break;
                case "Storia": file = "storia.png"; break;
                case "Scienze motorie": file = "scienze_motorie.png"; break;
                case "Telecomunicazioni": file = "telecomunicazioni.png"; break;
                case "Religione": file = "religione.png"; break;
                default: file = "sconosciuta.png"; break;
            }

            // Costruisce il percorso della risorsa grafica concatenando
            // la cartella Immagini con il nome del file PNG.
            // Il prefisso "pack://application:,,,/" indica che l'immagine
            // è una risorsa incorporata nell'applicazione WPF.
            // Affinché il caricamento funzioni, il file PNG deve essere
            // incluso nel progetto Visual Studio con la proprietà
            // Build Action/Azione di compilazione = Resource
            return new BitmapImage(
                new Uri("pack://application:,,,/Immagini/" + file,
                        UriKind.Absolute));
        }
    }
}
