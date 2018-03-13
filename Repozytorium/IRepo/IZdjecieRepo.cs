using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repozytorium.Models;

namespace Repozytorium.IRepo
{
  public interface IZdjecieRepo
  {
    void AddImage(Zdjecie img); 
    void DeleteImageByBlobName(string blobName); 
    List<Zdjecie> GetAllImages(string userId); 
    void SaveChanges();  
  }
}