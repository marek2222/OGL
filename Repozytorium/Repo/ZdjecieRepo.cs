using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repozytorium.IRepo;
using Repozytorium.Models;

namespace Repozytorium.Repo
{
  public class ZdjecieRepo : IZdjecieRepo
  { 
    private IOglContext _db;
    public ZdjecieRepo(IOglContext db)
    {
      _db = db;
    }

    public void AddImage(Zdjecie img)
    {
      _db.Zdjecia.Add(img);
    }

    public void DeleteImageByBlobName(string blobName)
    {
      Zdjecie img = _db.Zdjecia.Where(p=>p.Name == blobName).FirstOrDefault();
      _db.Zdjecia.Remove(img);
    }

    public List<Zdjecie> GetAllImages(string userId)
    {
      return _db.Zdjecia.Where(p=>p.UzytkownikId == userId).ToList();
    }

    public void SaveChanges()
    {
      _db.SaveChanges();
    }

  }
}