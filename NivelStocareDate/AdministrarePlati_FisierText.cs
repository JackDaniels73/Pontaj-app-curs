using System.Collections.Generic;
using System.IO;

namespace PontajPlatiCursuri;

public class AdministrarePlati_FisierText : IStocarePlati
{
    private readonly string _numeFisier;

    public AdministrarePlati_FisierText(string numeFisier)
    {
        _numeFisier = numeFisier;
        using (Stream streamFisierText = File.Open(_numeFisier, FileMode.OpenOrCreate))
        {
        }
    }

    public void AddPlata(Plata plata)
    {
        using (StreamWriter streamWriterFisierText = new StreamWriter(_numeFisier, true))
        {
            streamWriterFisierText.WriteLine(plata.ConversieLaSir_PentruFisier());
        }
    }

    public List<Plata> GetPlati()
    {
        List<Plata> plati = new List<Plata>();
        using (StreamReader streamReader = new StreamReader(_numeFisier))
        {
            string linieFisier;
            while ((linieFisier = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(linieFisier))
                {
                    plati.Add(new Plata(linieFisier));
                }
            }
        }
        return plati;
    }
}
