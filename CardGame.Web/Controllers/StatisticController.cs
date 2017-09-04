using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardGame.Web.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult Bestseller()
        {
            using (var cont = new CardGame_v2Entities())
            {
                            
                return View();
            }

        }
    }
}