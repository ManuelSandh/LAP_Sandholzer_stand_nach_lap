using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.Web.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        public ActionResult Overview()
        {
            List<DAL.Model.Card> CardList = new List<DAL.Model.Card>();

            var dbCardlist = CardManager.GetAllCards();

            foreach (var c in dbCardlist)
            {
                DAL.Model.Card card = new DAL.Model.Card();
                card.ID = c.ID;
                card.Name = c.Name;
                card.ManaCost = c.ManaCost;
                card.Attack = c.Attack;
                card.Life = c.Life;
                //card.Type = c.tbltype.typename;
                //card.Type = CardManager.GetCardTypeById(c.fktype);
                card.ID_CardType = CardManager.CardTypes[(int)c.ID_CardType];

                CardList.Add(card);
            }

            return View(CardList);
        }

        public ActionResult Details(int id)
        {
            DAL.Model.Card dbcard = null;

            dbcard = CardManager.GetCardById(id);

            DAL.Model.Card card = new DAL.Model.Card();
            card.ID = dbcard.ID;
            card.Name = dbcard.Name;
            card.ManaCost = dbcard.ManaCost;
            card.Attack = dbcard.Attack;
            card.Life = dbcard.Life;
            card.ID_CardType = CardManager.CardTypes[(int)dbcard.ID_CardType];

            return View(card);
        }     

    }
}