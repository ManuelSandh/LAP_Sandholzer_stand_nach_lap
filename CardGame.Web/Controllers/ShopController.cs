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
            shop.cardPacks = new List<CardPackModel>();
            shop.diamantPacks = new List<DiamantenModel>();

            var dbCardPacks = ShopManager.getAllCardPacks();
            var dbDiamantenPacks = ShopManager.getAllDiamantenPacks();

            foreach (var cont in dbCardPacks)
            {
                CardPackModel cardPack = new CardPackModel();
                cardPack.CardPackID = cont.ID;
                cardPack.PackName = cont.PackName;
                cardPack.NumCards = cont.NumCards;
                cardPack.Price = cont.PackPrice;
                shop.cardPacks.Add(cardPack);
            }
            foreach (var cont in dbDiamantenPacks)
            {
                DiamantenModel diamantenPack = new DiamantenModel();
                diamantenPack.DiamantenPackId = cont.ID;
                diamantenPack.Diamanten = (int)cont.Diamanten;
                diamantenPack.Price = cont.PackPrice;
                shop.diamantPacks.Add(diamantenPack);
            }
            return View(shop);
        }

        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult Buy(int? id)
        {

            return RedirectToAction("ShopIndex", "Shop");
        }

        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult BuyDiamanten(int? id)
        {
            return RedirectToAction("ShopIndex", "Shop");
        }
    }
}