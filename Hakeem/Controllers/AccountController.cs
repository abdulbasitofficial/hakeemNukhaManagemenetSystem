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
    public class AccountController : Controller
    {
        // GET: Account
        hakeemEntities db = new hakeemEntities();
        DateTime Now_Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Account acc)
        {
            var login = db.accounts.Where(e => e.account_email == acc.Email && e.account_passeord == acc.Password).FirstOrDefault();
            if (login!=null)
            {
                Session["id"] = login.account_id;
                Session["name"] = login.account_name;
                Session["email"] = login.account_email;
                Session["type"] = login.account_type;
                if (Session["type"].ToString().Equals("Hakeem"))
                {
                    return RedirectToAction("Nuskha", "Nuskha");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                ViewBag.Message = "Login Fialed ! Incorrect Record";
            }
            return View();
           
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Account acc)
        {
            if (ModelState.IsValid)
            {
                account ac = new account();
                ac.account_name = acc.Name;
                ac.account_email = acc.Email;
                ac.account_passeord = acc.Password;
                ac.account_type = acc.Type;
                ac.account_insertion = Now_Date;
                db.accounts.Add(ac);
                db.SaveChanges();
                return RedirectToAction("SignIn");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult LogOut()
        {
            Session.RemoveAll(); //Clear all session variables
            return RedirectToAction("Index", "Home");
        }
    }
}