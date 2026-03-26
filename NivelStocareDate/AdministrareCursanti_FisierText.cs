using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PontajPlatiCursuri;

public class AdministrareCursanti_FisierText : IStocareCursanti
{
    private readonly string _numeFisier;

    public AdministrareCursanti_FisierText(string numeFisier)
    {
        _numeFisier = numeFisier;
        // Se incearca deschiderea fisierului in modul OpenOrCreate
        using (Stream streamFisierText = File.Open(_numeFisier, FileMode.OpenOrCreate))
        {
        }
    }

    public void AddCursant(Cursant cursant)
    {
        using (StreamWriter streamWriterFisierText = new StreamWriter(_numeFisier, true))
        {
            streamWriterFisierText.WriteLine(cursant.ConversieLaSir_PentruFisier());
        }
    }

    public List<Cursant> GetCursanti()
    {
        List<Cursant> cursanti = new List<Cursant>();
        using (StreamReader streamReader = new StreamReader(_numeFisier))
        {
            string linieFisier;
            while ((linieFisier = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(linieFisier))
                {
                    cursanti.Add(new Cursant(linieFisier));
                }
            }
        }
        return cursanti;
    }

    // Functie de modificare care rescrie fisierul cu informatiile actualizate
    public void UpdateCursant(Cursant cursantActualizat)
    {
        var cursanti = GetCursanti();
        var index = cursanti.FindIndex(c => c.Id == cursantActualizat.Id);
        if (index != -1)
        {
            cursanti[index] = cursantActualizat;
        }

        // Rescriem tot continutul cu false (nu facem append, ci overwrite complet)
        using (StreamWriter streamWriterFisierText = new StreamWriter(_numeFisier, false))
        {
            foreach (var c in cursanti)
            {
                streamWriterFisierText.WriteLine(c.ConversieLaSir_PentruFisier());
            }
        }
    }

    // Funcție de Căutare - folosim Linq cum a fost cerut la exercitiul temei acasă
    public Cursant CautaCursant(string numeCautat)
    {
        var toti = GetCursanti();
        
        // Utilizare Linq pentru căutare și returnare
        var selectat = toti.Where(cursant => cursant.Nume.ToLower().Contains(numeCautat.ToLower()));
        
        return selectat.FirstOrDefault();
    }
}
