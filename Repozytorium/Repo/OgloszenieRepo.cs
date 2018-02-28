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

    public Ogloszenie GetOgloszenieById(int id)
    {
      Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
      return ogloszenie;
    }

    public void UsunOgloszenie(int id)
    {
      UsunPowiazanieOgloszenieKategoria(id);
      Ogloszenie ogloszenie = _db.Ogloszenia.Find(id);
      _db.Ogloszenia.Remove(ogloszenie);
    }

    private void UsunPowiazanieOgloszenieKategoria(int idOgloszenia)
    {
      var list = _db.Ogloszenie_Kategoria.Where(x => x.OgloszenieId == idOgloszenia);
      foreach (var el in list)
      {
        _db.Ogloszenie_Kategoria.Remove(el);
      }
    }

    public void SaveChanges()
    {
        _db.SaveChanges();
    }

  }
}