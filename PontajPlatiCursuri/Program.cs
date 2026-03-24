using PontajPlatiCursuri;

var registru = new RegistruCurs();

while (true)
{
    AfiseazaMeniu();
    var optiune = Console.ReadLine()?.Trim();

    switch (optiune)
    {
        case "1":
            AdaugaCursant(registru);
            break;
        case "2":
            AfiseazaCursanti(registru);
            break;
        case "3":
            InregistreazaPrezenta(registru);
            break;
        case "4":
            AdaugaPlata(registru);
            break;
        case "5":
            Cautare(registru);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Optiune invalida.");
            break;
    }

    Console.WriteLine();
}

static void AfiseazaMeniu()
{
    Console.WriteLine("=== Sistem Pontaj si Gestiune Plati (1 curs/grupa) ===");
    Console.WriteLine("1. Adaugare cursant");
    Console.WriteLine("2. Afisare cursanti");
    Console.WriteLine("3. Inregistrare prezenta (scade 1 sedinta)");
    Console.WriteLine("4. Adaugare plata (adauga sedinte)");
    Console.WriteLine("5. Cautare");
    Console.WriteLine("0. Iesire");
    Console.Write("Alege o optiune: ");
}

static void AdaugaCursant(RegistruCurs registru)
{
    var nume = ReadNonEmpty("Nume cursant: ");
    var sedinte = ReadInt("Sold initial (sedinte platite): ", min: 0);
    
    Console.WriteLine("Nivel curs (0=Incepator, 1=Intermediar, 2=Avansat): ");
    var nivelInt = ReadInt("Alege nivel: ", min: 0, max: 2);
    var nivel = (NivelCurs)nivelInt;

    Console.WriteLine("Optiuni (combina numere: 0=Fara, 1=Online, 2=Weekend, 4=Echipament): ");
    var optiuniInt = ReadInt("Suma optiuni: ", min: 0, max: 7);
    var optiuni = (OptiuniCursant)optiuniInt;

    var cursant = registru.AdaugaCursant(nume, sedinte, nivel, optiuni);
    Console.WriteLine($"Cursant adaugat: {cursant.Nume} (ID: {cursant.Id}) | {nivel} | {optiuni}");
}

static void AfiseazaCursanti(RegistruCurs registru)
{
    var lista = registru.ObtineSituatieFinanciara().ToList();
    if (lista.Count == 0)
    {
        Console.WriteLine("Nu exista cursanti.");
        return;
    }

    Console.WriteLine("Cursanti:");
    foreach (var s in lista)
    {
        var avertizare = s.PlataNecesara ? " | PLATA NECESARA" : string.Empty;
        Console.WriteLine($"- {s.Nume} | nivel: {s.Nivel} | optiuni: {s.Optiuni} | sedinte ramase: {s.SedinteRamase}{avertizare} | ID: {s.CursantId}");
    }
}

static void InregistreazaPrezenta(RegistruCurs registru)
{
    var cursant = AlegeCursant(registru);
    if (cursant is null)
    {
        return;
    }

    var data = ReadDateOnly("Data prezentei (YYYY-MM-DD, gol = azi): ");
    var aScazut = registru.InregistreazaPrezenta(cursant.Id, data);

    Console.WriteLine(aScazut
        ? "Prezenta inregistrata. S-a scazut o sedinta."
        : "Prezenta inregistrata, dar cursantul are sold 0 (nu s-a putut scadea)."
    );
}

static void AdaugaPlata(RegistruCurs registru)
{
    var cursant = AlegeCursant(registru);
    if (cursant is null)
    {
        return;
    }

    var data = ReadDateOnly("Data platii (YYYY-MM-DD, gol = azi): ");
    var sedinte = ReadInt("Sedinte cumparate: ", min: 1);

    registru.AdaugaPlata(cursant.Id, data, sedinte);
    Console.WriteLine("Plata inregistrata.");
}

static void Cautare(RegistruCurs registru)
{
    if (registru.Cursanti.Count == 0)
    {
        Console.WriteLine("Nu exista cursanti.");
        return;
    }

    Console.WriteLine("Cautare:");
    Console.WriteLine("1. Dupa nume (contine)");
    Console.WriteLine("2. Sold 0 (plata necesara)");
    Console.WriteLine("3. Sedinte ramase <= prag");
    Console.Write("Alege criteriul: ");

    var opt = Console.ReadLine()?.Trim();
    IEnumerable<SituatieFinanciaraCursant> rezultate;

    switch (opt)
    {
        case "1":
        {
            var q = ReadNonEmpty("Text cautare nume: ");
            rezultate = registru.ObtineSituatieFinanciara()
                .Where(x => x.Nume.Contains(q, StringComparison.CurrentCultureIgnoreCase));
            break;
        }
        case "2":
            rezultate = registru.ObtineSituatieFinanciara().Where(x => x.PlataNecesara);
            break;
        case "3":
        {
            var prag = ReadInt("Prag sedinte (>=0): ", min: 0);
            rezultate = registru.ObtineSituatieFinanciara().Where(x => x.SedinteRamase <= prag);
            break;
        }
        default:
            Console.WriteLine("Criteriu invalid.");
            return;
    }

    var list = rezultate.ToList();
    if (list.Count == 0)
    {
        Console.WriteLine("Niciun rezultat.");
        return;
    }

    Console.WriteLine("Rezultate:");
    foreach (var s in list)
    {
        var avertizare = s.PlataNecesara ? " | PLATA NECESARA" : string.Empty;
        Console.WriteLine($"- {s.Nume} | nivel: {s.Nivel} | optiuni: {s.Optiuni} | sedinte ramase: {s.SedinteRamase}{avertizare} | ID: {s.CursantId}");
    }
}

static Cursant? AlegeCursant(RegistruCurs registru)
{
    if (registru.Cursanti.Count == 0)
    {
        Console.WriteLine("Nu exista cursanti.");
        return null;
    }

    Console.WriteLine("Cursanti disponibili:");
    for (var i = 0; i < registru.Cursanti.Count; i++)
    {
        var c = registru.Cursanti[i];
        Console.WriteLine($"{i + 1}. {c.Nume} (sedinte ramase: {c.SedinteRamase})");
    }

    var index = ReadInt("Alege numarul cursantului: ", min: 1, max: registru.Cursanti.Count);
    return registru.Cursanti[index - 1];
}

static int ReadInt(string prompt, int min, int? max = null)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();

        if (!int.TryParse(txt, out var value))
        {
            Console.WriteLine("Valoare invalida.");
            continue;
        }

        if (value < min || (max.HasValue && value > max.Value))
        {
            Console.WriteLine(max.HasValue
                ? $"Valoarea trebuie sa fie intre {min} si {max.Value}."
                : $"Valoarea trebuie sa fie >= {min}.");
            continue;
        }

        return value;
    }
}

static string ReadNonEmpty(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(txt))
        {
            return txt.Trim();
        }

        Console.WriteLine("Valoare obligatorie.");
    }
}

static DateOnly ReadDateOnly(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var txt = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(txt))
        {
            return DateOnly.FromDateTime(DateTime.Today);
        }

        if (DateOnly.TryParse(txt.Trim(), out var d))
        {
            return d;
        }

        Console.WriteLine("Data invalida. Format acceptat: YYYY-MM-DD");
    }
}
