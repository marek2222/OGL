using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repozytorium.Models;

namespace Repozytorium.IRepo
{
  public interface IKategoriaRepo
  {
    IQueryable<Kategoria> PobierzKategorie();
    IQueryable<Ogloszenie> PobierzOgloszeniaZKategorii(int id);
  }
}
