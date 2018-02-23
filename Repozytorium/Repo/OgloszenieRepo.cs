using System.Diagnostics;
using System.Linq;
using Repozytorium.IRepo;
using Repozytorium.Models;

namespace Repozytorium.Repo
{
  public class OgloszenieRepo : IOgloszenieRepo
  {
    private readonly IOglContext _db;
    public OgloszenieRepo(IOglContext db)
    {
      _db = db;
    }

    public IQueryable<Ogloszenie> PobierzOgloszenia()
    {
      _db.Database.Log = message => Trace.WriteLine(message);
      var ogloszenia = _db.Ogloszenia.AsNoTracking();
      return ogloszenia;
    }
  }
}