using PontajPlatiCursuri;

var registru = new RegistruCurs();

var ana = registru.AdaugaCursant("Ana Pop", sedinteInitiale: 2);
var matei = registru.AdaugaCursant("Matei Ionescu", sedinteInitiale: 1);

var azi = DateOnly.FromDateTime(DateTime.Today);

registru.InregistreazaPrezenta(ana.Id, azi);
registru.InregistreazaPrezenta(matei.Id, azi);

registru.InregistreazaPrezenta(ana.Id, azi.AddDays(7));

registru.AdaugaPlata(matei.Id, azi.AddDays(1), sedinteCumparate: 4);

AfiseazaSituatie(registru);

static void AfiseazaSituatie(RegistruCurs registru)
{
    Console.WriteLine("Situatie financiara:");

    foreach (var s in registru.ObtineSituatieFinanciara())
    {
        var avertizare = s.PlataNecesara ? " | PLATA NECESARA" : string.Empty;
        Console.WriteLine($"- {s.Nume}: sedinte ramase = {s.SedinteRamase}{avertizare}");
    }
}
