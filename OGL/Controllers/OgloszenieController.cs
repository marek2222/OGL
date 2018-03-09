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
using Microsoft.AspNet.Identity;
using PagedList;

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
    public ActionResult Index(int? page, string sortOrder, string szukajTresc, string szukajTytul)
    {
      int currentPage = page ?? 1;
      int naStronie = 3;

      ViewBag.CurrentSort = sortOrder;
      ViewBag.IdSort = String.IsNullOrEmpty(sortOrder) ? "IdAsc" : "";
      ViewBag.DataDodaniaSort = sortOrder == "DataDodania" ? "DataDodaniaAsc" : "DataDodania";
      ViewBag.TrescSort = sortOrder == "TrescAsc" ? "Tresc" : "TrescAsc";
      ViewBag.TytulSort = sortOrder == "TytulAsc" ? "Tytul" : "TytulAsc";

      var ogloszenia = _repo.PobierzOgloszenia();
      // Wyszukiwanie
      ogloszenia = ogloszenia.Where(x => x.Tresc.Contains(szukajTresc) || szukajTresc == null);
      ogloszenia = ogloszenia.Where(x => x.Tytul.Contains(szukajTytul) || szukajTytul == null);

      switch (sortOrder)
      {
        case "DataDodania":
          ogloszenia = ogloszenia.OrderByDescending(s => s.DataDodania);
          break;
        case "DataDodaniaAsc":
          ogloszenia = ogloszenia.OrderBy(s => s.DataDodania);
          break;
        case "Tytul":
          ogloszenia = ogloszenia.OrderByDescending(s => s.Tytul);
          break;
        case "TytulAsc":
          ogloszenia = ogloszenia.OrderBy(s => s.Tytul);
          break;
        case "Tresc":
          ogloszenia = ogloszenia.OrderByDescending(s => s.Tresc);
          break;
        case "TrescAsc":
          ogloszenia = ogloszenia.OrderBy(s => s.Tresc);
          break;
        case "IdAsc":
          ogloszenia = ogloszenia.OrderBy(s => s.Id);
          break;
        default:    // id descending
          ogloszenia = ogloszenia.OrderByDescending(s => s.Id);
          break;
      }

      return View(ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
    }


    [OutputCache(Duration = 1000)]
    public ActionResult MojeOgloszenia(int? page)
    {
      int currentPage = page ?? 1;
      int naStronie = 3;
      string userId = User.Identity.GetUserId();

      var ogloszenia = _repo.PobierzOgloszenia();
      ogloszenia = ogloszenia.OrderByDescending(d => d.DataDodania).Where(o=>o.UzytkownikId == userId);

      return View(ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
    }

    public ActionResult Partial(int? page)
    {
      int currentPage = page ?? 1;
      int naStronie = 3;

      var ogloszenia = _repo.PobierzOgloszenia();
      ogloszenia = ogloszenia.OrderByDescending(d => d.DataDodania);
      return PartialView("Index", ogloszenia.ToPagedList<Ogloszenie>(currentPage, naStronie));
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
    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    // POST: /Ogloszenie/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "Tresc,Tytul")] Ogloszenie ogloszenie)
    {
      // Sprawdzanie typów
      if (ModelState.IsValid)
      {
        ogloszenie.UzytkownikId = User.Identity.GetUserId();
        ogloszenie.DataDodania = DateTime.Now;
        try
        {
          _repo.Dodaj(ogloszenie);
          _repo.SaveChanges();
          //return RedirectToAction("Index");   - poprzednio
          return RedirectToAction("MojeOgloszenia");
        }
        catch (Exception ex)
        {
          return View(ogloszenie);
        }
      }
      return View(ogloszenie);
    }

    // GET: /Ogloszenie/Edit/5  z autoryzacją
    [Authorize]
    public ActionResult Edit(int? id)
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
      else if (ogloszenie.UzytkownikId != User.Identity.GetUserId()
          && !(User.IsInRole("Admin") || User.IsInRole("Pracownik")))
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      //ViewBag.UzytkownikId = new SelectList(db.Users, "Id", "Email", ogloszenie.UzytkownikId);
      return View(ogloszenie);
    }

    // POST: /Ogloszenie/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,Tresc,Tytul,DataDodania,UzytkownikId")] Ogloszenie ogloszenie)
    {
      if (ModelState.IsValid)
      {
        try
        {
          _repo.Aktualizuj(ogloszenie);
          _repo.SaveChanges();
          //return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
          ViewBag.Blad = true;
          return View(ogloszenie);
        }
      }
      ViewBag.Blad = false;
      return View(ogloszenie);
    }

    // GET: /Ogloszenie/Delete/5
    [Authorize]
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
      // sprawdzenie, czy aktualnie zalogowany użytkownik jest właścicielem lub adminem. 
      // Jeśli tak, to może przejść do widoku usuwania. 
      // Jeśli nie, zostanie zwrócony komunikat 400 — Bad Request
      else if (ogloszenie.UzytkownikId != User.Identity.GetUserId() && User.IsInRole("Admin"))
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      if (blad != null)
        ViewBag.Blad = true;

      return View(ogloszenie);
    }

    // POST: /Ogloszenie/Delete/5
    [Authorize]
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
        return RedirectToAction("Delete", new { id = id, blad = true });
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
