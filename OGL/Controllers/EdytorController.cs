using Ganss.XSS;
using Microsoft.AspNet.Identity;
using Repozytorium.Models;
using System.Linq;
using System.Web.Mvc;

namespace OGL.Controllers
{
  public class EdytorController : Controller
  {
    public ActionResult EdytujTresc()
    {
      using (OglContext context = new OglContext())
      {
        string userId = User.Identity.GetUserId();
        Edytor edytor = context.Edytor.Where(x => x.Id == userId).FirstOrDefault();
        return View(edytor);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateInput(false)]
    public ActionResult EdytujTresc([Bind(Include = "Id,Tresc")]Edytor edytor)
    {
      if (ModelState.IsValid)
      {
        var userId = User.Identity.GetUserId();
        using (OglContext context = new OglContext())
        {
          var sanitizer = new HtmlSanitizer();
          var trescSprawdzona = sanitizer.Sanitize(edytor.Tresc);
          edytor.Tresc = trescSprawdzona;
          edytor.Id = userId;
          if (context.Edytor.Where(x => x.Id == userId).Any())
          {
            context.Entry(edytor).State = System.Data.Entity.EntityState.Modified;
          }
          else
          {
            context.Edytor.Add(edytor);
          }
          context.SaveChanges();
          return RedirectToAction("EdytujTresc", new { edytowanoDodanoInfo = 2 });
        }
      }
      ViewBag.Warning = "Coś poszło nie tak - spróbuj ponownie";
      return View(edytor);
    }

  }
}