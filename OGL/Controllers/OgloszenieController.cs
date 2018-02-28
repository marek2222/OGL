using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytorium.Models;
using System.Diagnostics;
using Repozytorium.Repo;
using Repozytorium.IRepo;


namespace OGL.Controllers
{
  public class OgloszenieController : Controller
  {

    private readonly IOgloszenieRepo _repo;
    public OgloszenieController(IOgloszenieRepo repo)
    {
      _repo = repo;
    }

    // GET: /Ogloszenie/
    public ActionResult Index()
    {
      var ogloszenia = _repo.PobierzOgloszenia();
      return View(ogloszenia.ToList());
    }


    // GET: /Ogloszenie/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
      if (ogloszenie == null)
      {
        return HttpNotFound();
      }
      return View(ogloszenie);
    }

    // GET: /Ogloszenie/Create
    public ActionResult Create()
    {
      //ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email");
      return View();
    }

    // POST: /Ogloszenie/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
    {
      if (ModelState.IsValid)
      {
        db.Ogloszenia.Add(ogloszenie);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId);
      return View(ogloszenie);
    }

    //// GET: /Ogloszenie/Edit/5
    //public ActionResult Edit(int? id)
    //{
    //  if (id == null)
    //  {
    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //  }
    //  Ogloszenie ogloszenie = db.Ogloszenia.Find(id);
    //  if (ogloszenie == null)
    //  {
    //    return HttpNotFound();
    //  }
    //  ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId);
    //  return View(ogloszenie);
    //}

    //// POST: /Ogloszenie/Edit/5
    //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public ActionResult Edit([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
    //{
    //  if (ModelState.IsValid)
    //  {
    //    db.Entry(ogloszenie).State = EntityState.Modified;
    //    db.SaveChanges();
    //    return RedirectToAction("Index");
    //  }
    //  ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId);
    //  return View(ogloszenie);
    //}

    // GET: /Ogloszenie/Delete/5
    public ActionResult Delete(int? id, bool? blad)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Ogloszenie ogloszenie = _repo.GetOgloszenieById((int)id);
      if (ogloszenie == null)
      {
        return HttpNotFound();
      }
      if (blad != null) 
        ViewBag.Blad = true;

      return View(ogloszenie);
    }

    // POST: /Ogloszenie/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      _repo.UsunOgloszenie(id);
      try
      {
        _repo.SaveChanges();
      }
      catch (Exception ex)
      {
        return RedirectToAction("Delete", new {id = id, blad = true});
      }
      return RedirectToAction("Index");
    }

    //protected override void Dispose(bool disposing)
    //{
    //  if (disposing)
    //  {
    //    db.Dispose();
    //  }
    //  base.Dispose(disposing);
    //}
  }
}
