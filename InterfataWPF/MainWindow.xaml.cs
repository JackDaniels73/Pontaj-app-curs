using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using PontajPlatiCursuri;

namespace InterfataWPF;

public partial class MainWindow : Window
{
    private AdministrareCursanti_FisierText _adminCursanti;
    private const int LUNGIME_MAXIMA_NUME = 30;

    public MainWindow()
    {
        InitializeComponent();

        string numeFisier = ConfigurationManager.AppSettings["NumeFisier"] ?? "cursanti.txt";
        _adminCursanti = new AdministrareCursanti_FisierText(numeFisier);

        IncarcaCursanti();
    }

    private void IncarcaCursanti(string filtruNume = null)
    {
        var cursanti = _adminCursanti.GetCursanti();
        
        if (cursanti.Count == 0 && string.IsNullOrEmpty(filtruNume))
        {
            _adminCursanti.AddCursant(new Cursant(Guid.NewGuid(), "Ion Popescu", 5, NivelCurs.Intermediar, OptiuniCursant.Online));
            _adminCursanti.AddCursant(new Cursant(Guid.NewGuid(), "Maria Ionescu", 2, NivelCurs.Incepator, OptiuniCursant.FaraOptiuni));
            cursanti = _adminCursanti.GetCursanti();
        }

        if (!string.IsNullOrWhiteSpace(filtruNume))
        {
            cursanti = cursanti.Where(c => c.Nume.Contains(filtruNume, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        lstCursanti.ItemsSource = cursanti;
    }

    private void LstCursanti_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (lstCursanti.SelectedItem is Cursant cursantSelectat)
        {
            lblNume.Text = cursantSelectat.Nume;
            
            // Populează câmpurile de editare
            cmbEditNivel.SelectedIndex = (int)cursantSelectat.Nivel;
            txtEditSedinte.Text = cursantSelectat.SedinteRamase.ToString();
            
            chkEditOnline.IsChecked = cursantSelectat.Optiuni.HasFlag(OptiuniCursant.Online);
            chkEditWeekend.IsChecked = cursantSelectat.Optiuni.HasFlag(OptiuniCursant.Weekend);
            chkEditFizic.IsChecked = cursantSelectat.Optiuni.HasFlag(OptiuniCursant.Fizic);

            // Activează controalele
            gridEditareDetalii.IsEnabled = true;
            btnUpdate.IsEnabled = true;
        }
        else
        {
            lblNume.Text = "Selectează un cursant";
            gridEditareDetalii.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            
            cmbEditNivel.SelectedIndex = -1;
            txtEditSedinte.Text = "";
            chkEditOnline.IsChecked = false;
            chkEditWeekend.IsChecked = false;
            chkEditFizic.IsChecked = false;
        }
    }

    private void BtnCauta_Click(object sender, RoutedEventArgs e)
    {
        string textCautat = txtCauta.Text.Trim();
        IncarcaCursanti(textCautat);
    }

    private bool ValideazaDateCursant(string nume, string sedinte, out int sedinteParsate)
    {
        bool esteValid = true;
        sedinteParsate = 0;
        List<string> mesajeEroare = new List<string>();

        lblMesajEroare.Visibility = Visibility.Collapsed;
        lblMesajEroare.Text = "";

        // Numele poate fi null in cazul actualizarii daca nu editam numele, dar aici verificam
        if (nume != null && string.IsNullOrWhiteSpace(nume))
        {
            mesajeEroare.Add("Numele nu poate fi gol.");
            esteValid = false;
        }
        else if (nume != null && nume.Length > LUNGIME_MAXIMA_NUME)
        {
            mesajeEroare.Add($"Numele nu poate depăși {LUNGIME_MAXIMA_NUME} caractere.");
            esteValid = false;
        }

        if (string.IsNullOrWhiteSpace(sedinte) || !int.TryParse(sedinte, out sedinteParsate) || sedinteParsate < 0)
        {
            mesajeEroare.Add("Numărul de ședințe trebuie să fie un număr întreg pozitiv sau zero.");
            esteValid = false;
        }

        if (!esteValid)
        {
            lblMesajEroare.Text = "Erori:\n" + string.Join("\n", mesajeEroare);
            lblMesajEroare.Visibility = Visibility.Visible;
            MessageBox.Show(lblMesajEroare.Text, "Eroare validare", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                NivelCurs nivel = NivelCurs.Incepator;
                if (rbIntermediar.IsChecked == true) nivel = NivelCurs.Intermediar;
                else if (rbAvansat.IsChecked == true) nivel = NivelCurs.Avansat;

                OptiuniCursant optiuni = OptiuniCursant.FaraOptiuni;
                if (chkOnline.IsChecked == true) optiuni |= OptiuniCursant.Online;
                if (chkWeekend.IsChecked == true) optiuni |= OptiuniCursant.Weekend;
                if (chkFizic.IsChecked == true) optiuni |= OptiuniCursant.Fizic;

                var cursantNou = new Cursant(Guid.NewGuid(), nume, sedinteParsate, nivel, optiuni);
                _adminCursanti.AddCursant(cursantNou);
                
                txtCauta.Text = "";
                IncarcaCursanti();
                BtnReset_Click(sender, e);
                
                MessageBox.Show("Cursant adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void BtnUpdate_Click(object sender, RoutedEventArgs e)
    {
        if (lstCursanti.SelectedItem is Cursant cursantSelectat)
        {
            string sedinteStr = txtEditSedinte.Text;

            if (ValideazaDateCursant(null, sedinteStr, out int sedinteParsate))
            {
                try
                {
                    NivelCurs nivelNou = (NivelCurs)cmbEditNivel.SelectedIndex;

                    OptiuniCursant optiuniNoi = OptiuniCursant.FaraOptiuni;
                    if (chkEditOnline.IsChecked == true) optiuniNoi |= OptiuniCursant.Online;
                    if (chkEditWeekend.IsChecked == true) optiuniNoi |= OptiuniCursant.Weekend;
                    if (chkEditFizic.IsChecked == true) optiuniNoi |= OptiuniCursant.Fizic;

                    var cursantActualizat = new Cursant(
                        cursantSelectat.Id,
                        cursantSelectat.Nume,
                        sedinteParsate,
                        nivelNou,
                        optiuniNoi
                    );

                    _adminCursanti.UpdateCursant(cursantActualizat);

                    IncarcaCursanti(txtCauta.Text.Trim());
                    MessageBox.Show("Cursant actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la actualizare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private void BtnSterge_Click(object sender, RoutedEventArgs e)
    {
        if (lstCursanti.SelectedItem is Cursant cursantSelectat)
        {
            MessageBoxResult raspuns = MessageBox.Show(
                $"Ești sigur că vrei să ștergi cursantul '{cursantSelectat.Nume}'?",
                "Confirmare Ștergere",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (raspuns == MessageBoxResult.Yes)
            {
                try
                {
                    _adminCursanti.DeleteCursant(cursantSelectat.Id);
                    IncarcaCursanti(txtCauta.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la ștergere: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        else
        {
            MessageBox.Show("Te rog să selectezi un cursant din listă mai întâi.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        txtNume.Text = string.Empty;
        txtSedinte.Text = string.Empty;
        txtCauta.Text = string.Empty;
        
        rbIncepator.IsChecked = true;
        chkOnline.IsChecked = false;
        chkWeekend.IsChecked = false;
        chkFizic.IsChecked = false;
        
        lblMesajEroare.Visibility = Visibility.Collapsed;
        lblMesajEroare.Text = "";

        IncarcaCursanti();
    }
}