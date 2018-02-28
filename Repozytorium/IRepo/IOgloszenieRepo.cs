using System.Linq;
using Repozytorium.Models;

namespace Repozytorium.IRepo
{
  public interface IOgloszenieRepo
  {
    IQueryable<Ogloszenie> PobierzOgloszenia();
    Ogloszenie GetOgloszenieById(int id);
    void Dodaj(Ogloszenie ogloszenie);
    void UsunOgloszenie(int id);
    void SaveChanges();
  }
}