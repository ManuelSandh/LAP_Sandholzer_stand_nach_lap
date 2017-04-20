using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;

namespace CardGame.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.Name != "")
            {
                ViewBag.Username = User.Identity.Name;

                var dbUser = UserManager.GetUserByEmail(User.Identity.Name);
                ViewBag.Gamertag = dbUser.gamertag;
                ViewBag.CurrencyBalance = dbUser.currency;
                
            }
            

            return View();
        }
        
        public ActionResult Statistics()
        {
            Statistic s = new Statistic();

            //Befülle die Statistik
            s.NumUsers = DBInfoManager.GetNumUsers();
            s.NumCards = DBInfoManager.GetNumCards();
            s.NumDecks = DBInfoManager.GetNumDecks();

            return View(s);
        }


        public ActionResult Shop()
        {
            return View();
        }


        public ActionResult Impressum()
        {            
            return View();
        }

        public ActionResult AGB()
        {
            return View();
        }
    }
}