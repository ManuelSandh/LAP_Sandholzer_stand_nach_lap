using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Model;
using CardGame.DAL.Logic;
using CardGame.DAL;
using CardGame.Log;
using System.Diagnostics;

namespace CardGame.Web.Controllers
{
    public class ShopController : Controller
    {
        //GET: Shop
        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult ShopIndex()
        {
            Shop shop = new Shop();
            shop.cardPacks = new List<CardPackModel>();
            shop.diamantPacks = new List<DiamantenModel>();


            var dbCardPacks = ShopManager.getAllAktiveCardPacks();
            var dbDiamantenPacks = ShopManager.getAllDiamantenPacks();

            foreach (var cont in dbCardPacks)
            {
                CardPackModel cardPack = new CardPackModel();
                cardPack.CardPackID = cont.ID;
                cardPack.PackName = cont.PackName;
                cardPack.NumCards = cont.NumCards;
                cardPack.Price = cont.PackPrice;
                cardPack.AveragePack = GetRating(cont.ID);
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

            if (UserManager.GetUserByEmail(User.Identity.Name).AmountMoney >= ShopManager.GetCardPackById(idCardPack).PackPrice)
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

        [HttpPost]
        [Authorize(Roles = "player,admin")]
        public ActionResult AddRating(string ratingSubmit, int? star)
        {
            int parsedRatingSubmit;

            parsedRatingSubmit = Int32.Parse(ratingSubmit);

            DAL.Logic.ShopManager.saveRatinginDB(parsedRatingSubmit, star);
            return RedirectToAction("ShopIndex");
        }


        [Authorize(Roles = "player,admin")]
        public double GetRating(int id)
        {
            double result = DAL.Logic.ShopManager.GetPackRatingAverageById(id);
            return result;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DatenPflege()
        {
            List<CardPack> dbCardpacks = ShopManager.getAllCardPacks();

            CardPackViewModel viewCardPack;
            List<CardPackViewModel> listeCardPackViewModel = new List<CardPackViewModel>();

            foreach (CardPack dbCardPack in dbCardpacks)
            {
                viewCardPack = new CardPackViewModel();

                viewCardPack.PacketName = dbCardPack.PackName;
                viewCardPack.Anzahlkarten = dbCardPack.NumCards;
                viewCardPack.Preis = dbCardPack.PackPrice;
                viewCardPack.Aktiv = (bool)dbCardPack.Aktiv;
                viewCardPack.IDCardPack = dbCardPack.ID;

                listeCardPackViewModel.Add(viewCardPack);
            }

            return View(listeCardPackViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditAktivCardpack(int id)
        {
            CardPack dbCardPack = ShopManager.GetCardPackById(id);

            CardPackViewModel viewCardPack = new CardPackViewModel();

            viewCardPack.IDCardPack = dbCardPack.ID;
            viewCardPack.PacketName = dbCardPack.PackName;
            viewCardPack.Anzahlkarten = dbCardPack.NumCards;
            viewCardPack.Preis = dbCardPack.PackPrice;
            viewCardPack.Aktiv = (bool)dbCardPack.Aktiv;

            return View(viewCardPack);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditAktivCardpack(CardPackViewModel viewCardPack)
        {
            ShopManager.UpdateCardPackAktiv(viewCardPack.IDCardPack, viewCardPack.Aktiv);

            return RedirectToAction("Datenpflege");
        }

        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult ChangePassword(ChangePasswordModel passModel)
        {
            return View();
        }
    }
}
