using System.Collections.Generic;

namespace PontajPlatiCursuri;

public interface IStocarePlati
{
    void AddPlata(Plata plata);
    List<Plata> GetPlati();
}
