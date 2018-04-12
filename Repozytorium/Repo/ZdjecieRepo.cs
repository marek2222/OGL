using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repozytorium.IRepo;
using Repozytorium.Models;
using System.Web.Mvc;

namespace Repozytorium.Repo
{
  public class ZdjecieRepo : IZdjecieRepo
  { 
    private IOglContext _db;
    public ZdjecieRepo(IOglContext db)
    {
      _db = db;
    }

    public SelectList GetUser(string user)
    {
      if (user == null)
        return new SelectList(_db.Uzytkownicy, "Id", "Email");
      else 
        return new SelectList(_db.Uzytkownicy, "Id", "Email", user);
    }

    public void AddImage(Zdjecie img)
    {
      _db.Zdjecia.Add(img);
    }

    public Zdjecie FindImageById(int? id)
    {
      return _db.Zdjecia.Where(p => p.Id == id).SingleOrDefault();
    }

    public void DeleteImageById(int? id)
    {
      Zdjecie img = _db.Zdjecia.Where(p => p.Id == id).FirstOrDefault();
      _db.Zdjecia.Remove(img);
    }

    public void DeleteImageByBlobName(string blobName)
    {
      Zdjecie img = _db.Zdjecia.Where(p=>p.Name == blobName).FirstOrDefault();
      _db.Zdjecia.Remove(img);
    }

    public List<Zdjecie> GetAllImages()
    {
      return _db.Zdjecia.ToList();
    }

    public void SaveChanges()
    {
      _db.SaveChanges();
    }

  }
}