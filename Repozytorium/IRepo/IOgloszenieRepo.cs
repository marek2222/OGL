using System.Linq;
using Repozytorium.Models;

namespace Repozytorium.IRepo
{
  public interface IOgloszenieRepo
  {
    IQueryable<Ogloszenie> PobierzOgloszenia();
  }
}