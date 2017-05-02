using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Model;
using CardGame.DAL.Logic;
using CardGame.Log;

namespace CardGame.Web.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
       
        public ActionResult Index()
        {            
            return View();
        }     
    }
}