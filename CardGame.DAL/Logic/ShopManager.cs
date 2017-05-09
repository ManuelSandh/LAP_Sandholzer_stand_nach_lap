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


        public static void ExecuteOrder(int idPerson, int idPack)
        {
            using (var db = new CardGame_v2Entities())
            {
                UserCardCollection coll = new UserCardCollection();
                Random rnd = new Random();

                int cardq = (from q in db.AllCardPacks
                             where q.ID == idPack
                             select q.NumCards).FirstOrDefault();

                var updatePerson = (from u in db.AllUsers
                                    where u.ID == idPack
                                    select u);

                var packValue = (from v in db.AllCardPacks
                                 where v.ID == idPack
                                 select v.PackPrice).FirstOrDefault();

                foreach (var value in updatePerson)
                {
                    value.AmountMoney -= (packValue);
                }
                db.SaveChanges();

                for (int i = 0; i < cardq; i++)
                {
                    int rng = rnd.Next(1, 698);
                    var card = (from c in db.AllCards
                                where c.ID == rng
                                select c).FirstOrDefault();

                    if (card != null)
                    {
                        coll.ID_User = idPerson;
                        coll.ID_Card = card.ID;

                        db.AllUserCardCollections.Add(coll);
                        db.SaveChanges();
                    }
                    else
                    {
                        i = i - 1;
                    }
                }
            }
            

            //Generiere Random Karten
            //Random rdm = new Random();
            //var generatedCards = new List<Card>();

            //try
            //{
            //    using (var cont = new CardGame_v2Entities())
            //    {
            //        var cardPack = cont.AllCardPacks.Find(id);

            //        if (cardPack == null)
            //        {
            //            throw new Exception("CardPackNotFound");
            //        }
            //        int numOfCardsToGenerate = cardPack.NumCards;

            //        for (int i = 0; i < numOfCardsToGenerate; ++i)
            //        {
            //            int rng = rdm.Next(1,680);
            //            var card = (from c in cont.AllCards
            //                        where c.ID == rng
            //                        select c).FirstOrDefault();

            //            if (card != null)
            //            {
            //                generatedCards.Add(generatedCards[i]);
            //            }
            //            else
            //            {
            //                i = i - 1;
            //            }
            //        }
            //    }
            //}
           
        }
    }
}
