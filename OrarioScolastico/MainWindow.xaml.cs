using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OrarioClasse;

namespace OrarioClasse;

public partial class MainWindow : Window
{
    private readonly OrarioController controller = new();
    private readonly Dictionary<Posizione, Button> pulsanti = new();
    private static readonly string[] Giorni = { "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì" };

    public MainWindow()
    {
        InitializeComponent();
        CreaGriglia();
        AggiornaVista();
    }

    private void CreaGriglia()
    {
        GrigliaOrario.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(55) });
        for (int g = 0; g < Orario.Giorni; g++)
            GrigliaOrario.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        GrigliaOrario.RowDefinitions.Add(new RowDefinition { Height = new GridLength(38) });
        for (int o = 0; o < Orario.Ore; o++)
            GrigliaOrario.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        for (int g = 0; g < Orario.Giorni; g++)
            AggiungiTesto(Giorni[g], 0, g + 1);
        for (int o = 0; o < Orario.Ore; o++)
        {
            AggiungiTesto($"{o + 1}ª", o + 1, 0);
            for (int g = 0; g < Orario.Giorni; g++)
            {
                var posizione = new Posizione(g, o);
                var bottone = new Button
                {
                    Tag = posizione,
                    Margin = new Thickness(3),
                    Padding = new Thickness(4),
                    BorderThickness = new Thickness(2)
                };
                bottone.Click += Cella_Click;
                Grid.SetRow(bottone, o + 1);
                Grid.SetColumn(bottone, g + 1);
                GrigliaOrario.Children.Add(bottone);
                pulsanti[posizione] = bottone;
            }
        }
    }

    private void AggiungiTesto(string testo, int riga, int colonna)
    {
        var etichetta = new TextBlock
        {
            Text = testo,
            FontWeight = FontWeights.SemiBold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetRow(etichetta, riga);
        Grid.SetColumn(etichetta, colonna);
        GrigliaOrario.Children.Add(etichetta);
    }

    private void Cella_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button { Tag: Posizione posizione }) return;
        controller.Seleziona(posizione);
        EsitoText.Text = "";
        AggiornaVista();
    }

    private void Scambia_Click(object sender, RoutedEventArgs e)
    {
        var esito = controller.RichiediScambio();
        EsitoText.Text = esito.Messaggio;
        EsitoText.Foreground = esito.Autorizzato ? Brushes.DarkGreen : Brushes.Firebrick;
        AggiornaVista();
    }

    private void AggiornaVista()
    {
        foreach (var (posizione, bottone) in pulsanti)
        {
            var tessera = controller.Orario.Leggi(posizione);
            bottone.Content = tessera is null ? "(vuota)" :
                new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = tessera.Materia, FontWeight = FontWeights.SemiBold, TextAlignment = TextAlignment.Center, TextWrapping = TextWrapping.Wrap },
                        new TextBlock { Text = tessera.Aula, TextAlignment = TextAlignment.Center, Foreground = Brushes.DimGray }
                    }
                };
            bool selezionato = controller.Prima == posizione || controller.Seconda == posizione;
            bottone.Background = selezionato ? Brushes.LightSkyBlue : tessera is null ? Brushes.Gainsboro : Brushes.AliceBlue;
            bottone.BorderBrush = selezionato ? Brushes.DodgerBlue : Brushes.LightGray;
        }
        ScambiaButton.IsEnabled = controller.Pronto;
        SelezioneText.Text = controller.Prima is null ? "Seleziona la prima cella." :
            controller.Seconda is null ? "Seleziona la seconda cella." : "Due celle selezionate: premi Scambia.";
    }
}
