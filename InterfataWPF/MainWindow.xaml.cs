using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using PontajPlatiCursuri;

namespace InterfataWPF;

public partial class MainWindow : Window
{
    private AdministrareCursanti_FisierText _adminCursanti;
    private const int LUNGIME_MAXIMA_NUME = 15;

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

    private bool ValideazaDateCursant(string nume, string sedinte, out int sedinteParsate)
    {
        bool esteValid = true;
        sedinteParsate = 0;
        List<string> mesajeEroare = new List<string>();

        // Resetare culori 
        var brushNormal = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#94A3B8"));
        var brushEroare = System.Windows.Media.Brushes.Red;

        lblNumeInput.Foreground = brushNormal;
        lblSedinteInput.Foreground = brushNormal;
        lblMesajEroare.Visibility = Visibility.Collapsed;
        lblMesajEroare.Text = "";

        if (string.IsNullOrWhiteSpace(nume))
        {
            lblNumeInput.Foreground = brushEroare;
            mesajeEroare.Add("Numele nu poate fi gol.");
            esteValid = false;
        }
        else if (nume.Length > LUNGIME_MAXIMA_NUME)
        {
            lblNumeInput.Foreground = brushEroare;
            mesajeEroare.Add($"Numele nu poate depăși {LUNGIME_MAXIMA_NUME} caractere.");
            esteValid = false;
        }

        if (string.IsNullOrWhiteSpace(sedinte) || !int.TryParse(sedinte, out sedinteParsate) || sedinteParsate <= 0)
        {
            lblSedinteInput.Foreground = brushEroare;
            mesajeEroare.Add("Numărul de ședințe trebuie să fie un număr întreg pozitiv.");
            esteValid = false;
        }

        if (!esteValid)
        {
            lblMesajEroare.Text = "Date invalide:\n" + string.Join("\n", mesajeEroare);
            lblMesajEroare.Visibility = Visibility.Visible;
        }

        return esteValid;
    }

    private void BtnAdauga_Click(object sender, RoutedEventArgs e)
    {
        string nume = txtNume.Text;
        string sedinteStr = txtSedinte.Text;

        if (ValideazaDateCursant(nume, sedinteStr, out int sedinteParsate))
        {
            try
            {
                var cursantNou = new Cursant(System.Guid.NewGuid(), nume, sedinteParsate);
                _adminCursanti.AddCursant(cursantNou);
                
                // Refresh list
                IncarcaCursanti();
                
                // Clear form
                BtnReset_Click(sender, e);
                
                MessageBox.Show("Cursant adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Eroare la salvare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        txtNume.Text = string.Empty;
        txtSedinte.Text = string.Empty;
        
        var brushNormal = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#94A3B8"));
        lblNumeInput.Foreground = brushNormal;
        lblSedinteInput.Foreground = brushNormal;
        
        lblMesajEroare.Visibility = Visibility.Collapsed;
        lblMesajEroare.Text = "";
    }
}