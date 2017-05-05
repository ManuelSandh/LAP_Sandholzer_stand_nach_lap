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
    public class ShopManager
    {
        public static List<CardPack> getAllCardPacks()
        {
            var allCardPacks = new List<CardPack>();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    allCardPacks = db.AllCardPacks.ToList();
                }
                if (allCardPacks == null)
                    throw new Exception("NoCardPacksFound");
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
            return allCardPacks;
        }

        public static List<DiamantenPack> getAllDiamantenPacks()
        {
            var allDiamantenPacks = new List<DiamantenPack>();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    allDiamantenPacks = db.AllDiamantenPacks.ToList();
                }
                if (allDiamantenPacks == null)
                    throw new Exception("NoCardPacksFound");
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
            return allDiamantenPacks;
        }

        public static CardPack GetCardPackById(int id)
        {
            var dbCardPack = new CardPack();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbCardPack = db.AllCardPacks.Find(id);
                }
                if (dbCardPack == null)
                    throw new Exception("CardPackNotFound");
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
            return dbCardPack;
        }

        public static DiamantenPack GetDiamantenPackById(int id)
        {
            var dbDiamantenPack = new DiamantenPack();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbDiamantenPack = db.AllDiamantenPacks.Find(id);
                }
                if (dbDiamantenPack == null)
                    throw new Exception("CardPackNotFound");
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
            return dbDiamantenPack;
        }


        public static List<Card> generateCards(int id)
        {
            //Generiere Random Karten
            Random rdm = new Random();
            var generatedCards = new List<Card>();

            try
            {
                using (var cont = new CardGame_v2Entities())
                {
                    var cardPack = cont.AllCardPacks.Find(id);

                    if (cardPack == null)
                    {
                        throw new Exception("CardPackNotFound");
                    }
                    int numOfCardsToGenerate = cardPack.NumCards;

                    for (int i = 0; i < numOfCardsToGenerate; ++i)
                    {
                        //int rng = r.Next(1, 698);
                        //var card = (from c in cont.AllCards
                        //            where c.idcard == rng
                        //            select c).FirstOrDefault();

                        //if (card != null)
                        //{
                        //    col.fkperson = personID;
                        //    col.fkorder = orderID;
                        //    col.fkcard = card.idcard;

                        //    db.tblcollection.Add(col);
                        //    db.SaveChanges();
                        //}
                        //else
                        //{
                        //    i = i - 1;
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
                
            return generatedCards;
        }
    }
}
