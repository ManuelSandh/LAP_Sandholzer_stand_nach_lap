﻿using System;
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
                return RedirectToAction("BestellungAbgeschlossen","Shop");
            }
            else
                TempData["ErrorMessage"] = "Nicht genug Diamanten";

            return RedirectToAction("ShopIndex");
        }

        [HttpPost]
        [Authorize(Roles ="player,admin")]
        public ActionResult DiamantenpackErwerben(int idDiamantenPack)
        {

            //hole Diamenteninfos über id
            DiamantenPack diamanten = ShopManager.GetDiamantenPackById(idDiamantenPack);

            KreditKartenZahlungVM kvm = new KreditKartenZahlungVM();
            kvm.Preis = diamanten.PackPrice;
            kvm.AnzahlDiamanten = (int)diamanten.Diamanten;
            kvm.IdDiamantenPack = diamanten.ID;
            //return RedirectToAction("KreditKartenZahlung", idDiamantenPack);

            return View("KreditKartenZahlung",kvm);
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

            bool hatFunktioniert = DAL.Logic.ShopManager.saveRatinginDB(parsedRatingSubmit, star, User.Identity.Name);
            if (!hatFunktioniert)
            {
                TempData["ErrorMessage"] = "Sie haben dieses Pack bereits bewertet";

            }        

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


        public ActionResult KreditKartenZahlung()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult KreditKartenZahlung(KreditKartenZahlungVM vm)
        {
            var result = DAL.Logic.Helper.IsCCValid(vm.KartenNr);

            if (result == true)
            {
                //Hier muss man nun anhand der Diamantenpackid den Eintrag in die DB machen
                //1. Hole alle Diamantenpackinfos
                DiamantenPack diamantenpack = ShopManager.GetDiamantenPackById(vm.IdDiamantenPack);

                //2. Addiere die Anzahl an Diamanten vom Diamantenpack zum User
                ShopManager.AddDiamondsToUser(User.Identity.Name, (int)diamantenpack.Diamanten);

                //3. Trage den Kauf des Diamantenpacks in VirtualPurchases ein
                ShopManager.InsertVirtualDiamondPurchase(User.Identity.Name, vm.IdDiamantenPack);
               
                TempData["ConfirmMessage"] = "Sie haben" + " " + diamantenpack.Diamanten + " "+ "Diamanten gekauft";
                return RedirectToAction("BestellungDanke", "Shop");
            }
            else
            {
                TempData["ErrorMessage"] = "Ungültige Kartennummer!";
                return View(vm);
            }         
        }
        
        public ActionResult BestellungAbgeschlossen()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Rechnung(int id)
        {
            //TODO brauche noch den aktuellen Preises des Packs - Card oder Diamanten, und die bezeichnung des Packs
            RechnungVM rvm = new RechnungVM();          
            CardPack cardpack = ShopManager.GetCardPackById(id);
            var user = UserManager.GetUserByEmail(User.Identity.Name);
            rvm.Vorname = user.FirstName;
            rvm.Nachname = user.LastName;
            rvm.Pack = cardpack.PackName;
            //rvm.Preis = ;
            

            return View(rvm);
        }

    }
}
