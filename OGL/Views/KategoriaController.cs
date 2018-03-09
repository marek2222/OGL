using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repozytorium.Models;

namespace OGL.Views
{
    public class KategoriaController : Controller
    {
        private OglContext db = new OglContext();

        // GET: /Kategoria/
        public ActionResult Index()
        {
            return View(db.Kategorie.ToList());
        }

    }
}
