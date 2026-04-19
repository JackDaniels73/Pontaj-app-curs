using System.Configuration;
using System.Windows;
using PontajPlatiCursuri;

namespace InterfataWPF;

public partial class MainWindow : Window
{
    private AdministrareCursanti_FisierText _adminCursanti;

    public MainWindow()
    {
        InitializeComponent();

        // Preluarea numelui fisierului din App.config
        string numeFisier = ConfigurationManager.AppSettings["NumeFisier"] ?? "cursanti.txt";
        _adminCursanti = new AdministrareCursanti_FisierText(numeFisier);

        IncarcaCursanti();
    }

    private void IncarcaCursanti()
    {
        var cursanti = _adminCursanti.GetCursanti();
        
        // Daca fisierul este gol, cream niste date de test
        if (cursanti.Count == 0)
        {
            _adminCursanti.AddCursant(new Cursant(System.Guid.NewGuid(), "Ion Popescu", 5, NivelCurs.Intermediar, OptiuniCursant.Online));
            _adminCursanti.AddCursant(new Cursant(System.Guid.NewGuid(), "Maria Ionescu", 2, NivelCurs.Incepator, OptiuniCursant.FaraOptiuni));
            cursanti = _adminCursanti.GetCursanti();
        }

        lstCursanti.ItemsSource = cursanti;
    }

    private void LstCursanti_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (lstCursanti.SelectedItem is Cursant cursantSelectat)
        {
            lblNume.Text = cursantSelectat.Nume;
            lblNivel.Text = $"Nivel: {cursantSelectat.Nivel}";
            lblSedinte.Text = $"Ședințe Rămase: {cursantSelectat.SedinteRamase}";
            lblOptiuni.Text = $"Opțiuni: {cursantSelectat.Optiuni}";
        }
    }
}