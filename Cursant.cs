namespace PontajPlatiCursuri;

public sealed class Cursant
{
    public Cursant(Guid id, string nume, int sedinteRamase)
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
    }

    public Guid Id { get; }
    public string Nume { get; }

    public int SedinteRamase { get; private set; }

    internal void AdaugaSedinte(int sedinte)
    {
        if (sedinte <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sedinte), "Numarul de sedinte trebuie sa fie pozitiv.");
        }

        SedinteRamase += sedinte;
    }

    internal bool IncearcaScadereSedinta()
    {
        if (SedinteRamase <= 0)
        {
            return false;
        }

        SedinteRamase--;
        return true;
    }
}
