namespace PontajPlatiCursuri;

public sealed class RegistruCurs
{
    private readonly List<Cursant> _cursanti = new();
    private readonly List<Prezenta> _prezente = new();
    private readonly List<Plata> _plati = new();

    public IReadOnlyList<Cursant> Cursanti => _cursanti;
    public IReadOnlyList<Prezenta> Prezente => _prezente;
    public IReadOnlyList<Plata> Plati => _plati;

    public Cursant AdaugaCursant(string nume, int sedinteInitiale, NivelCurs nivel = NivelCurs.Incepator, OptiuniCursant optiuni = OptiuniCursant.FaraOptiuni)
    {
        var cursant = new Cursant(Guid.NewGuid(), nume, sedinteInitiale, nivel, optiuni);
        _cursanti.Add(cursant);
        return cursant;
    }

    public bool InregistreazaPrezenta(Guid cursantId, DateOnly data)
    {
        var cursant = GasesteCursant(cursantId);

        var scazut = cursant.IncearcaScadereSedinta();
        _prezente.Add(new Prezenta(cursantId, data));

        return scazut;
    }

    public void AdaugaPlata(Guid cursantId, DateOnly data, int sedinteCumparate)
    {
        var cursant = GasesteCursant(cursantId);

        cursant.AdaugaSedinte(sedinteCumparate);
        _plati.Add(new Plata(cursantId, data, sedinteCumparate));
    }

    public IEnumerable<SituatieFinanciaraCursant> ObtineSituatieFinanciara()
    {
        return _cursanti
            .OrderBy(x => x.Nume, StringComparer.CurrentCultureIgnoreCase)
            .Select(c => new SituatieFinanciaraCursant(c.Id, c.Nume, c.SedinteRamase, c.SedinteRamase == 0, c.Nivel, c.Optiuni));
    }

    private Cursant GasesteCursant(Guid cursantId)
    {
        var cursant = _cursanti.FirstOrDefault(x => x.Id == cursantId);
        if (cursant is null)
        {
            throw new InvalidOperationException("Cursant inexistent.");
        }

        return cursant;
    }
}

public sealed record SituatieFinanciaraCursant(Guid CursantId, string Nume, int SedinteRamase, bool PlataNecesara, NivelCurs Nivel, OptiuniCursant Optiuni);
