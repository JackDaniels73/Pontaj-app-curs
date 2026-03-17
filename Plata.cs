namespace PontajPlatiCursuri;

public sealed class Plata
{
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
}
