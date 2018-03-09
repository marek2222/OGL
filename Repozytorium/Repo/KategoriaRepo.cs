using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Repozytorium.IRepo;
using Repozytorium.Models;

namespace Repozytorium.Repo
{
  public class KategoriaRepo : IKategoriaRepo
  {
    private readonly IOglContext _db;
    public KategoriaRepo (IOglContext db)
	  {
      _db = db;
	  }

    public IQueryable<Kategoria> PobierzKategorie()
    {
      _db.Database.Log = message => Trace.WriteLine(message);
      var kategorie = _db.Kategorie.AsNoTracking();
      return kategorie;
    }

    public IQueryable<Ogloszenie> PobierzOgloszeniaZKategorii(int id)
    {
      _db.Database.Log = message => Trace.WriteLine(message);
      var ogloszania = from o in _db.Ogloszenia 
                       join k in _db.Ogloszenie_Kategoria on o.Id equals k.Id 
                       where k.KategoriaId == id 
                       select o;
      return ogloszania;
    }

  }
}