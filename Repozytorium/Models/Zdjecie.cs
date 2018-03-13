using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repozytorium.Models
{
  public class Zdjecie
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UzytkownikId { get; set; }
    public Uzytkownik Uzytkownik { get; set; }
  }
}