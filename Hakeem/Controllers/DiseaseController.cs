using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Hakeem.Models;
namespace Hakeem.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class DiseaseController : Controller
    {
        hakeemEntities db = new hakeemEntities();
        // GET: Symptoms
        [HttpGet]
        public ActionResult Disease()
        {
            List<Disease> file = new List<Disease>();
            var query = db.diseases.ToList();
            foreach(var item in query){
                file.Add(new Disease { ID=item.disease_id, Name = item.disease_name });
            }
            return View(file);
        }
        [HttpGet]
        public ActionResult Add()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Add(Disease sym)
        {
            disease ac = new disease();
            ac.disease_name = sym.Name;
            db.diseases.Add(ac);
            db.SaveChanges();
            return RedirectToAction("Disease");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Disease sym)
        {
            return View();
        }
    }
}