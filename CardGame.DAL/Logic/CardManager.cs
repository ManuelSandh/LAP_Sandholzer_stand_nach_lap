using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Data.Entity;

namespace CardGame.DAL.Logic
{
    public class CardManager
    {
        public static readonly Dictionary<int, string> CardTypes;

        static CardManager()
        {
            CardTypes = new Dictionary<int, string>();
            List<CardType> cardTypeList = null;

            using (var db = new CardGame_v2Entities())
            {
                cardTypeList = db.AllCardTypes.ToList();
            }

            foreach (var type in cardTypeList)
            {
                CardTypes.Add(type.ID, type.Name);
            }

            CardTypes.Add(0, "n/a");
        }

        public static List<Card> GetAllCards()
        {
            List<Card> ReturnList = null;
            using (var db = new CardGame_v2Entities())
            {
                //ReturnList = db.tblCard.Include(t => t.CardType).ToList();
                ReturnList = db.AllCards.ToList();
            }
            return ReturnList;
        }

        //Theoretisch überflüssig
        public static string GetCardTypeById(int? id)
        {
            string TypeName = "n/a";

            using (var db = new CardGame_v2Entities())
            {
                TypeName = db.AllCardTypes.Find(id).Name;
            }
            return TypeName;
        }

        public static Card GetCardById(int id)
        {
            Card card = null;

            using (var db = new CardGame_v2Entities())
            {
                //Extention Method
                card = db.AllCards.Where(c => c.ID == id).FirstOrDefault();

                //Klassisch LINQ
                //card = (from c in db.tblCard
                //        where c.idcard == id
                //        select c).FirstOrDefault();
            }
            return card;
        }

        public static List<Deck> GetAllDecks()
        {
            List<Deck> ReturnList = null;

            using (var db = new CardGame_v2Entities())
            {                
                ReturnList = db.AllDecks.ToList();
            }
            return ReturnList;
        }


        public static Card GetCard(int id)
        {
          
            Card card = null;

            if (id < 1)
                throw new ArgumentException("invalid card id", nameof(id));

            try
            {
                using (var context = new CardGame_v2Entities())
                {
                    card = context.AllCards.FirstOrDefault(x => x.ID == id);
                }
            }
            catch (Exception ex)
            {
               
                if (ex.InnerException != null)                   
                throw ex;
            }

            return card;
        }

    }
}

