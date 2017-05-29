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
        #region myCode


        //GET: Shop
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

        [HttpPost]
        [Authorize(Roles = "player,admin")]
        public ActionResult ShopIndex(int idCardPack)
        {

            int userID = UserManager.GetUserByEmail(User.Identity.Name).ID;

            if (UserManager.GetUserByEmail(User.Identity.Name).AmountMoney > ShopManager.GetCardPackById(idCardPack).PackPrice)
            {                
                ShopManager.ExecuteOrder(userID, idCardPack);
                TempData["ConfirmMessage"] = "Kauf erfolgreich ";
            }
            else
                TempData["ErrorMessage"] = "Nicht genug Diamanten";

            return RedirectToAction("ShopIndex");

        }

        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult Buy(int idCardPack)
        {
            var dbCardPack = ShopManager.GetCardPackById(idCardPack);

            CardPack cardPack = new CardPack();
            cardPack.ID = dbCardPack.ID;
            cardPack.PackName = dbCardPack.PackName;
            cardPack.NumCards = dbCardPack.NumCards;
            cardPack.PackPrice = dbCardPack.PackPrice;

            return View(cardPack);
        }

        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult BuyDiamanten(int id)
        {
            var dbDiamantenPack = ShopManager.GetDiamantenPackById(id);

            DiamantenPack diamantenpack = new DiamantenPack();
            diamantenpack.ID = dbDiamantenPack.ID;
            diamantenpack.Diamanten = dbDiamantenPack.Diamanten;
            diamantenpack.PackPrice = dbDiamantenPack.PackPrice;

            return View(diamantenpack);
        }

        #endregion
    }
}