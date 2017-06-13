using CardGame.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class DeckDetailModel
    {
        public DeckOverViewModel DeckOverview { get; set; }

        public List<CardModel> CardsForDeck { get; set; }

        public List<CardModel> CardsInDeck { get; set; }

    }
}