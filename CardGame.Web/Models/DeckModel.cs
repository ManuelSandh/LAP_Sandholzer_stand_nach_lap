using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class DeckModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<CardModel> CardsForDeck { get; set; }
        public List<CardModel> CardsInDeck { get; set; }

    }
}