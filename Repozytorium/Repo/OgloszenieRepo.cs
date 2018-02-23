using System.Diagnostics;
using System.Linq;
using Repozytorium.IRepo;
using Repozytorium.Models;

namespace Repozytorium.Repo
{
  public class OgloszenieRepo : IOgloszenieRepo
  {
    private OglContext db = new OglContext();
    public IQueryable<Ogloszenie> PobierzOgloszenia()
    {
      db.Database.Log = message => Trace.WriteLine(message);
      var ogloszenia = db.Ogloszenia.AsNoTracking();
      return ogloszenia;
    }
  }
}