using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Repozytorium.Models;

namespace Repozytorium.IRepo
{
  public interface IOglContext 
  {
    DbEntityEntry Entry(object entity);

    DbSet<Kategoria> Kategorie { get; set; }
    DbSet<Ogloszenie> Ogloszenia { get; set; }
    DbSet<Uzytkownik> Uzytkownik { get; set; }
    DbSet<Ogloszenie_Kategoria> Ogloszenie_Kategoria { get; set; }
    DbSet<Zdjecie> Zdjecia { get; set; }

    int SaveChanges();
    Database Database { get; }
  }
}
