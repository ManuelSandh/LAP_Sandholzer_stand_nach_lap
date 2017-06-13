using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardGame.Web.Controllers
{
    [Authorize]
    public class DeckBuilderController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            /// rufe Deckbuildermanager.GetDecks(..) mit dem angemeldeten User auf
            /// Erstelle Liste von DeckModel objekten 
      
           List<DeckModel> decks = new List<DeckModel>();
           try
            {
                List<Deck> userDecks = DeckBuilderManager.GetDecks(User.Identity.Name);

                foreach (var userDeck in userDecks)
                {
                    decks.Add(new DeckModel()
                    {
                        ID = userDeck.ID,
                        Name = userDeck.Name,                       
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            /// gib diese Liste an die View weiter
            return View(decks);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            List<Card> allCardsForDeck = DeckBuilderManager.GetCardsForDeck(id);
            List<Card> allCardsInDeck = DeckBuilderManager.GetCardsInDeck(id);
            Deck deck = DeckBuilderManager.GetDeck(id);

            DeckModel model = new DeckModel();
            model.Name = deck.Name;
            model.ID = deck.ID;
            model.CardsInDeck = new List<CardModel>();
            model.CardsForDeck = new List<CardModel>();

            foreach (var cardInDeck in allCardsInDeck)
            {
                model.CardsInDeck.Add(new CardModel()
                {
                    ID = cardInDeck.ID,
                    Mana = cardInDeck.ManaCost,
                    Attack = cardInDeck.Attack,
                    Life = cardInDeck.Life,
                    Name = cardInDeck.Name,
                    Type = cardInDeck.CardType.Name
                });
            }


            foreach (var cardForDeck in allCardsForDeck)
            {
                model.CardsForDeck.Add(new CardModel()
                {
                    ID = cardForDeck.ID,
                    Mana = cardForDeck.ManaCost,
                    Attack = cardForDeck.Attack,
                    Life = cardForDeck.Life,
                    Name = cardForDeck.Name,
                    Type = cardForDeck.CardType.Name
                });
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCardToDecK(int idDeck, int idCard)
        {
            DeckBuilderManager.AddCardToDeck(idDeck, idCard);

            return RedirectToAction("Show", new { id = idDeck });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCardFromDecK(int idDeck, int idCard)
        {
            DeckBuilderManager.RemoveCardFromDeck(idDeck, idCard);

            return RedirectToAction("Show", new { id = idDeck });
        }

    }
}