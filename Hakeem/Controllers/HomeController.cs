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
    public class HomeController : Controller
    {
        hakeemEntities db = new hakeemEntities();
        public ActionResult Index()
        {
            List<Nuskha> file = new List<Nuskha>();

            var query = db.nuskhas.Join(db.diseases, n => n.disease_id, d => d.disease_id, (n, d) => new { n, d }).Join(db.favourite_nuskha,nd=>nd.n.nuskha_id,f=>f.nuskha_id,(nd,f)=> new {nd, f }).OrderByDescending(m=>m.f.stars).ToList();
            foreach (var item in query)
            {
                file.Add(new Nuskha
                {
                    ID = item.nd.n.nuskha_id,
                    HakeemID = (int)item.nd.n.account_id,
                    Name = item.nd.n.nuskha_name,
                    Description = item.nd.n.nuskha_description,
                    Ingredients = item.nd.n.nuskha_ingredients,
                    Symptoms = item.nd.n.symptoms,
                    DiseaseName = item.nd.d.disease_name,
                    Imge = item.nd.n.nuskha_image,
                    Type = item.nd.n.nuskha_imgtype
                });
            }
            List<Account> list = new List<Account>();

            var userlist = db.accounts.ToList();
            foreach (var item in userlist)
            {
                list.Add(new Account { ID = item.account_id, Name = item.account_name });
            }
            ViewBag.User = list;
            ViewBag.StartAverage = StarsAverageNuskha();
            return View(file);
        }
        public List<Nuskha> StarsAverageNuskha()
        {
            List<Nuskha> list = new List<Nuskha>();

            var ratingCount = from f in db.favourite_nuskha.GroupBy(f => f.nuskha_id)
                     select new
                     {
                         count = f.Sum(c=>c.stars),
                         f.FirstOrDefault().nuskha_id,
                     };

            var userCount = db.favourite_nuskha.GroupBy(l => l.nuskha_id)
              .Select(g => new
              {
                  g.FirstOrDefault().nuskha_id,
                  Count = g.Select(l => l.account_id).Distinct().Count()
              });
            foreach (var item in ratingCount)
            {
                foreach (var file in userCount)
                {
                    if (item.nuskha_id==file.nuskha_id)
                    {
                        decimal st = Math.Floor((decimal)item.count / file.Count);
                        list.Add(new Nuskha {
                            ID = (int)item.nuskha_id,
                            Stars = st,
                            StarsNumber=(float)item.count,
                            TotalUsers = file.Count
                        });
                    }
                }
            }
            return list;
        }
            
        public ActionResult Search(string search)
        {
            List<Nuskha> file = new List<Nuskha>();

            var query = db.nuskhas.Join(db.diseases, n => n.disease_id, d => d.disease_id, (n, d) => new { n, d }).Join(db.favourite_nuskha, nd => nd.n.nuskha_id, f => f.nuskha_id, (nd, f) => new { nd, f }).Where(m => m.nd.d.disease_name.Contains(search) || m.nd.n.symptoms.Contains(search)).OrderByDescending(m => m.f.stars).ToList();
            foreach (var item in query)
            {
                file.Add(new Nuskha
                {
                    ID = item.nd.n.nuskha_id,
                    HakeemID = (int)item.nd.n.account_id,
                    Name = item.nd.n.nuskha_name,
                    Description = item.nd.n.nuskha_description,
                    Ingredients = item.nd.n.nuskha_ingredients,
                    Symptoms = item.nd.n.symptoms,
                    DiseaseName = item.nd.d.disease_name,
                    Imge = item.nd.n.nuskha_image,
                    Type = item.nd.n.nuskha_imgtype
                });
            }
            List<Account> list = new List<Account>();

            var userlist = db.accounts.ToList();
            foreach (var item in userlist)
            {
                list.Add(new Account { ID = item.account_id, Name = item.account_name });
            }
            ViewBag.User = list;
            ViewBag.StartAverage = StarsAverageNuskha();
            return View("Index", file);
        }
        public ActionResult Details(int id)
        {
            Nuskha nus = new Nuskha();
            var item = db.nuskhas.Join(db.diseases, n => n.disease_id, d => d.disease_id, (n, d) => new { n, d }).Where(m => m.n.nuskha_id == id).FirstOrDefault();
            nus.HakeemID = (int)item.n.account_id;
            nus.Name = item.n.nuskha_name;
            nus.Description = item.n.nuskha_description;
            nus.Ingredients = item.n.nuskha_ingredients;
            nus.Symptoms = item.n.symptoms;
            nus.DiseaseName = item.d.disease_name;
            nus.Imge = item.n.nuskha_image;
            nus.Type = item.n.nuskha_imgtype;

            nus.ID = id;


            List<Account> list = new List<Account>();
            var userlist = db.accounts.ToList();
            foreach (var file in userlist)
            {
                list.Add(new Account { ID = file.account_id, Name = file.account_name });
            }
            ViewBag.User = list;

            List<Nuskha> cmt = new List<Nuskha>();
            var cmtlist = db.nuskha_comment.Where(e=>e.nuskha_id==id).ToList();
            foreach (var file in cmtlist)
            {
                cmt.Add(new Nuskha { ID = (int)file.nuskha_id, AccountID = (int)file.account_id,Comments=file.comments });
            }
            ViewBag.Comments = cmt;
            int stars;
            if (Session["id"] != null)
            {
                int accid = (int)Session["id"];
                var query = db.favourite_nuskha.Where(f => f.account_id == accid && f.nuskha_id == id).FirstOrDefault();
                if (query != null)
                {
                    stars = (int)query.stars;
                    
                }
                else
                {
                    stars = 0;
                }
            }
            else
            {
                stars = 0;
            }

            ViewBag.Rated = stars;
            return View(nus);
        }
       
        [HttpPost]
        public ActionResult Comments(string id,string comments)
        {
            if(Session["id"]!=null)
            {
                int nuskhaId = int.Parse(id); 
                nuskha_comment cmt = new nuskha_comment();
                cmt.comments = comments;
                cmt.account_id = (int)Session["id"];
                cmt.nuskha_id = nuskhaId;
                db.nuskha_comment.Add(cmt);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = nuskhaId });
            }
            else
            {
                return RedirectToAction("SignIn","Account");
            }
            
        }
        public ActionResult Rating(int id, int star)
        {
            if (Session["id"] != null)
            {
                int uid = (int)Session["id"];
                var check = db.favourite_nuskha.Where(f => f.account_id == uid && f.nuskha_id == id).FirstOrDefault();
                if(check != null)
                {
                    check.stars = star;
                    db.SaveChanges();
                }
                else
                {
                    favourite_nuskha cmt = new favourite_nuskha();
                    cmt.nuskha_id = id;
                    cmt.account_id = (int)Session["id"];
                    cmt.stars = star;
                    db.favourite_nuskha.Add(cmt);
                    db.SaveChanges();
                }
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }

        }
        public ActionResult Hakeem()
        {

            List<Account> list = new List<Account>();
            var query = db.accounts.Where(e => e.account_type.Equals("Hakeem")).ToList();
            foreach (var item in query)
            {
                list.Add(new Account {
                    Name = item.account_name,
                    ID= item.account_id,
                });
            }

            List<Account> user = new List<Account>();
            var userCount = db.favourite_nuskha.Join(db.nuskhas,f=>f.nuskha_id,n=>n.nuskha_id,(f,n) => new {f,n }).GroupBy(l => l.n.account_id)
             .Select(g => new
             {
                 g.FirstOrDefault().n.account_id,
                 Count = g.Count(),
             });
            foreach (var item in userCount)
            {
                user.Add(new Account
                {
                    ID = (int)item.account_id,
                    count = item.Count
                });
            }


            ViewBag.StarAverage = StarsAverageHakeem();
            ViewBag.Hakeem = user;
            return View(list);
        }
        public ActionResult RatingHakeem(int id, int star)
        {
            if (Session["id"] != null)
            {
                int uid = (int)Session["id"];
                var check = db.favourite_hakeem.Where(f => f.account_id == uid && f.hakeem_id == id).FirstOrDefault();
                if (check != null)
                {
                    check.stars = star;
                    db.SaveChanges();
                }
                else
                {
                    favourite_hakeem cmt = new favourite_hakeem();
                    cmt.hakeem_id = id;
                    cmt.account_id = (int)Session["id"];
                    cmt.stars = star;
                    db.favourite_hakeem.Add(cmt);
                    db.SaveChanges();
                }
                return RedirectToAction("Hakeem");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }

        }
        public List<Account> StarsAverageHakeem()
        {
            List<Account> list = new List<Account>();

            var ratingCount = from f in db.favourite_hakeem.GroupBy(f => f.hakeem_id)
                              select new
                              {
                                  count = f.Sum(c => c.stars),
                                  f.FirstOrDefault().hakeem_id,
                              };

            var userCount = db.favourite_hakeem.GroupBy(l => l.hakeem_id)
              .Select(g => new
              {
                  g.FirstOrDefault().hakeem_id,
                  Count = g.Select(l => l.account_id).Distinct().Count()
              });
            foreach (var item in ratingCount)
            {
                foreach (var file in userCount)
                {
                    if (item.hakeem_id == file.hakeem_id)
                    {
                        decimal st = Math.Ceiling((decimal)item.count / file.Count);
                        list.Add(new Account { ID = (int)item.hakeem_id, Stars = st });
                    }
                }
            }
            return list;
        }
    }
}