using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repozytorium.Models;
using System.Web.Mvc;

namespace Repozytorium.IRepo
{
  public interface IZdjecieRepo
  {
    void AddImage(Zdjecie img);
    Zdjecie FindImageById(int? id);
    void DeleteImageById(int? id);
    List<Zdjecie> GetAllImages(); 
    void SaveChanges();
    SelectList GetUser(string user);
  }
}