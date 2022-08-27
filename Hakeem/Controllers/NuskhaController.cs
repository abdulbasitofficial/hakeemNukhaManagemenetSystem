using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Hakeem.Models;
namespace Hakeem.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class NuskhaController : Controller
    {
        hakeemEntities db = new hakeemEntities();
        // GET: Nuskha

        [HttpGet]
        public ActionResult Nuskha()
        {
            if (Session["id"] != null)
            {
                List<Nuskha> file = new List<Nuskha>();
            int id = int.Parse(Session["id"].ToString());

            var query = db.nuskhas.Join(db.diseases, n => n.disease_id, d => d.disease_id, (n, d) => new { n, d }).Where(m=> m.n.account_id==id).ToList();
            foreach (var item in query)
            {
                file.Add(new Nuskha {
               
                Name=item.n.nuskha_name,
                Description=item .n.nuskha_description,
                Ingredients= item.n.nuskha_ingredients,
                Symptoms=item.n.symptoms,
                DiseaseName=item.d.disease_name,
                Imge=item.n.nuskha_image,
                Type=item.n.nuskha_imgtype
                });;
            }
               
            return View(file);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        [HttpGet]
        public ActionResult AddNuskha()
        {
            Nuskha nus = new Nuskha();
            ViewBag.disease = DiseasesDropDownSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult AddNuskha(int id,Nuskha nus, HttpPostedFileBase image )
        {
            if(Session["id"]!=null)
            {
                byte[] bytes;
                BinaryReader br = new BinaryReader(image.InputStream);
                bytes = br.ReadBytes(image.ContentLength);
                nuskha ns = new nuskha();
                ns.nuskha_name = nus.Name;
                ns.nuskha_description = nus.Description;
                ns.disease_id = id;
                ns.symptoms = nus.Symptoms;
                ns.nuskha_image = bytes;
                ns.nuskha_imgtype = image.ContentType;
                ns.nuskha_ingredients = nus.Ingredients;
                ns.account_id = int.Parse(Session["id"].ToString());
                db.nuskhas.Add(ns);
                db.SaveChanges();

                return RedirectToAction("Nuskha");
            }
            else
            {
                return RedirectToAction("SignIn","Account");
            }
            
        }

        public List<Disease> DiseasesDropDownList()
        {
            
            List<Disease> dis = new List<Disease>();
            List<Disease> list = new List<Disease>();
            var query = db.diseases.ToList();
            foreach (var item in query)
            {
                list.Add(new Disease { ID = item.disease_id, Name = item.disease_name });
            }
            foreach (var item in list)
            {
                dis.Add(new Disease { ID = item.ID, Name = item.Name });
            }
            return dis;
        }
        public List<SelectListItem> DiseasesDropDownSelectList()
        {

            List<SelectListItem> dis = new List<SelectListItem>();
            List<Disease> list = new List<Disease>();
            var query = db.diseases.ToList();
            foreach (var item in query)
            {
                list.Add(new Disease { ID = item.disease_id, Name = item.disease_name });
            }
            foreach (var item in list)
            {
                dis.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Name });
            }
            return dis;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Nuskha nus)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Disble(int id)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Enable(int id)
        {
            return View();
        }

        
    }
}
