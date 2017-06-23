﻿using System;
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
                ViewBag.Gamertag = dbUser.GamerTag;
                ViewBag.CurrencyBalance = dbUser.AmountMoney;
                
            }
            

            return View();
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