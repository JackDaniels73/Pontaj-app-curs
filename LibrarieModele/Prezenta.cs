namespace PontajPlatiCursuri;

public sealed class Prezenta
{
    public Prezenta(Guid cursantId, DateOnly data)
    {
        CursantId = cursantId;
        Data = data;
    }

    public Guid CursantId { get; }
    public DateOnly Data { get; }
}
