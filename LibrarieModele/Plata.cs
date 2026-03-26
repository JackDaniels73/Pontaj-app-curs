namespace PontajPlatiCursuri;

public sealed class Plata
{
    private const char SEPARATOR_PRINCIPAL_FISIER = ';';

    public Plata(string linieFisier)
    {
        var dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

        CursantId = Guid.Parse(dateFisier[0]);
        Data = DateOnly.Parse(dateFisier[1]);
        SedinteCumparate = int.Parse(dateFisier[2]);
    }

    public Plata(Guid cursantId, DateOnly data, int sedinteCumparate)
    {
        if (sedinteCumparate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sedinteCumparate), "Numarul de sedinte cumparate trebuie sa fie pozitiv.");
        }

        CursantId = cursantId;
        Data = data;
        SedinteCumparate = sedinteCumparate;
    }

    public Guid CursantId { get; }
    public DateOnly Data { get; }
    public int SedinteCumparate { get; }

    public string ConversieLaSir_PentruFisier()
    {
        return string.Format("{1}{0}{2}{0}{3}",
            SEPARATOR_PRINCIPAL_FISIER,
            CursantId.ToString(),
            Data.ToString("yyyy-MM-dd"),
            SedinteCumparate.ToString());
    }
}
