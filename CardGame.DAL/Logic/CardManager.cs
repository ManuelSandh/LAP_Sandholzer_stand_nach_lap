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
            List<tblCardType> cardTypeList = null;

            using (var db = new CardGame_v2Entities())
            {
                cardTypeList = db.tblCardType.ToList();
            }

            foreach (var type in cardTypeList)
            {
                CardTypes.Add(type.idCardType, type.typename);
            }

            CardTypes.Add(0, "n/a");
        }

        public static List<tblCard> GetAllCards()
        {
            List<tblCard> ReturnList = null;
            using (var db = new CardGame_v2Entities())
            {
                //ReturnList = db.tblCard.Include(t => t.CardType).ToList();
                ReturnList = db.tblCard.ToList();
            }
            return ReturnList;

        }

        //Theoretisch überflüssig
        public static string GetCardTypeById(int? id)
        {
            string TypeName = "n/a";

            using (var db = new CardGame_v2Entities())
            {
                TypeName = db.tblCardType.Find(id).typename;
            }
            return TypeName;
        }

        public static tblCard GetCardById(int id)
        {
            tblCard card = null;

            using (var db = new CardGame_v2Entities())
            {
                //Extention Method
                card = db.tblCard.Where(c => c.idCard == id).FirstOrDefault();

                //Klassisch LINQ
                //card = (from c in db.tblCard
                //        where c.idcard == id
                //        select c).FirstOrDefault();
            }

            return card;
        }

        public static List<tblDeck> GetAllDecks()
        {
            List<tblDeck> ReturnList = null;

            using (var db = new CardGame_v2Entities())
            {                
                ReturnList = db.tblDeck.ToList();
            }
            return ReturnList;

        }
    }
}

