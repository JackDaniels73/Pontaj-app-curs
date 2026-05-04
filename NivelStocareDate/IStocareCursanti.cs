using System.Collections.Generic;

namespace PontajPlatiCursuri;

public interface IStocareCursanti
{
    void AddCursant(Cursant cursant);
    List<Cursant> GetCursanti();
    void UpdateCursant(Cursant cursant);
    Cursant CautaCursant(string nume);
    void DeleteCursant(System.Guid id);
}
