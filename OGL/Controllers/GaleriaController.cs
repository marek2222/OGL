using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytorium.Models;
using System.IO;
using Repozytorium.IRepo;
using Microsoft.AspNet.Identity;

namespace OGL.Controllers
{
  public class GaleriaController : Controller
  {
    private readonly IZdjecieRepo _repo;
    public GaleriaController(IZdjecieRepo repo)
    {
      _repo = repo;
    }

    // GET: Zdjecia
    public ActionResult Index()
    {
      List<Zdjecie> zdjecia = _repo.GetAllImages(); 
      return View(zdjecia);
    }

    // GET: Zdjecia/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Zdjecie zdjecie = _repo.FindImageById(id);
      if (zdjecie == null)
      {
        return HttpNotFound();
      }
      return View(zdjecie);
    }

    // GET: Zdjecia/Create
    public ActionResult Create()
    {
      ViewBag.UzytkownikId = _repo.GetUser(null);
      return View();
    }

    // POST: Zdjecia/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Name,UzytkownikId,ImageName,SizeName,ImgBytes")] ZdjeciePlik model)
    {
      var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };
      if (model.ImgBytes == null || model.ImgBytes.ContentLength == 0)
      {
        ModelState.AddModelError("ImgBytes", "To pole jest wymagane...");
      }
      else if (!imageTypes.Contains(model.ImgBytes.ContentType))
      {
        ModelState.AddModelError("ImgBytes", "Please choose either a GIF, JPG or PNG image.");
      }

      if (ModelState.IsValid)
      {
        var zdjecie = new Zdjecie();
        zdjecie.Name = model.Name;
        zdjecie.UzytkownikId = model.UzytkownikId;
        zdjecie.ImageName = model.ImageName;
        zdjecie.SizeName = model.SizeName;

        using (var binaryReader = new BinaryReader(model.ImgBytes.InputStream))
        {
          zdjecie.ImgBytes = binaryReader.ReadBytes(model.ImgBytes.ContentLength);
        }

        _repo.AddImage(zdjecie);
        _repo.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.UzytkownikId = _repo.GetUser(model.UzytkownikId);
      return View(model);
    }

    //// GET: Zdjecia/Edit/5
    //public ActionResult Edit(int? id)
    //{
    //  if (id == null)
    //  {
    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //  }
    //  Zdjecie zdjecie = db.Zdjecia.Find(id);
    //  if (zdjecie == null)
    //  {
    //    return HttpNotFound();
    //  }
    //  ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", zdjecie.UzytkownikId);
    //  return View(zdjecie);
    //}

    //// POST: Zdjecia/Edit/5
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit([Bind(Include = "Id,Name,UzytkownikId,ImageName,SizeName")] Zdjecie model)
    //{
    //  if (ModelState.IsValid)
    //  {
    //    db.Entry(model).State = EntityState.Modified;
    //    db.SaveChanges();
    //    return RedirectToAction("Index");
    //  }
    //  ViewBag.UzytkownikId = _repo.GetUser(User.Identity.GetUserId());

    //  return View(model);
    //}

    // GET: Zdjecia/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Zdjecie zdjecie = _repo.FindImageById(id);
      if (zdjecie == null)
      {
        return HttpNotFound();
      }
      return View(zdjecie);
    }

    // POST: Zdjecia/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      _repo.DeleteImageById(id);
      _repo.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}
