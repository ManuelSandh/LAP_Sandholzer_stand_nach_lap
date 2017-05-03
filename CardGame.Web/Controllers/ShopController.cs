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
        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult ShopIndex()
        {
            Shop shop = new Shop();
            shop.cardPacks = new List<Models.CardPack>();

            var dbCardPacks = ShopManager.getAllCardPacks();

            foreach (var dbCp in dbCardPacks)
            {
                DAL.Model.CardPack cardPack = new DAL.Model.CardPack();
                cardPack.ID = dbCp.ID;
                cardPack.PackName = dbCp.PackName;
                cardPack.NumCards = dbCp.NumCards;
                cardPack.PackPrice = dbCp.PackPrice;
                //TODO Fehler!
                //shop.cardPacks.Add(cardPack);
            }

            return View(shop);
        }
    }
}