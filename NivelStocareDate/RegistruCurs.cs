namespace PontajPlatiCursuri;

public sealed class RegistruCurs
{
    private readonly List<Cursant> _cursanti = new();
    private readonly List<Prezenta> _prezente = new();
    private readonly List<Plata> _plati = new();

    private readonly IStocareCursanti _stocareCursanti;
    private readonly IStocarePlati _stocarePlati;

    public RegistruCurs(IStocareCursanti stocareCursanti, IStocarePlati stocarePlati)
    {
        _stocareCursanti = stocareCursanti;
        _stocarePlati = stocarePlati;

        _cursanti.AddRange(_stocareCursanti.GetCursanti());
        _plati.AddRange(_stocarePlati.GetPlati());
    }

    public IReadOnlyList<Cursant> Cursanti => _cursanti;
    public IReadOnlyList<Prezenta> Prezente => _prezente;
    public IReadOnlyList<Plata> Plati => _plati;

    public Cursant AdaugaCursant(string nume, int sedinteInitiale, NivelCurs nivel = NivelCurs.Incepator, OptiuniCursant optiuni = OptiuniCursant.FaraOptiuni)
    {
        var cursant = new Cursant(Guid.NewGuid(), nume, sedinteInitiale, nivel, optiuni);
        _cursanti.Add(cursant);
        _stocareCursanti.AddCursant(cursant); // Salvare persistenta fisier
        return cursant;
    }

    public bool InregistreazaPrezenta(Guid cursantId, DateOnly data)
    {
        var cursant = GasesteCursant(cursantId);

        var scazut = cursant.IncearcaScadereSedinta();
        _prezente.Add(new Prezenta(cursantId, data));

        if (scazut)
        {
            _stocareCursanti.UpdateCursant(cursant); // Salvare persistenta date modificate
        }

        return scazut;
    }

    public void AdaugaPlata(Guid cursantId, DateOnly data, int sedinteCumparate)
    {
        var cursant = GasesteCursant(cursantId);

        cursant.AdaugaSedinte(sedinteCumparate);
        var plata = new Plata(cursantId, data, sedinteCumparate);
        _plati.Add(plata);

        _stocareCursanti.UpdateCursant(cursant); // Modificat cursant
        _stocarePlati.AddPlata(plata); // Noua inregistrare adaugata
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
