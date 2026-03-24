namespace PontajPlatiCursuri;

public enum NivelCurs
{
    Incepator,
    Intermediar,
    Avansat
}

[Flags]
public enum OptiuniCursant
{
    FaraOptiuni = 0,
    Online = 1,
    Weekend = 2,
    EchipamentInclus = 4
}

public sealed class Cursant
{
    public Cursant(Guid id, string nume, int sedinteRamase, NivelCurs nivel = NivelCurs.Incepator, OptiuniCursant optiuni = OptiuniCursant.FaraOptiuni)
    {
        if (string.IsNullOrWhiteSpace(nume))
        {
            throw new ArgumentException("Numele nu poate fi gol.", nameof(nume));
        }

        if (sedinteRamase < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sedinteRamase), "SedinteRamase nu poate fi negativ.");
        }

        Id = id;
        Nume = nume;
        SedinteRamase = sedinteRamase;
        Nivel = nivel;
        Optiuni = optiuni;
    }

    public Guid Id { get; }
    public string Nume { get; }
    public NivelCurs Nivel { get; set; }
    public OptiuniCursant Optiuni { get; set; }

    public int SedinteRamase { get; private set; }

    public void AdaugaSedinte(int sedinte)
    {
        if (sedinte <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sedinte), "Numarul de sedinte trebuie sa fie pozitiv.");
        }

        SedinteRamase += sedinte;
    }

    public bool IncearcaScadereSedinta()
    {
        if (SedinteRamase <= 0)
        {
            return false;
        }

        SedinteRamase--;
        return true;
    }
}
