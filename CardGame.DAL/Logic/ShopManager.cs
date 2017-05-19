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
    public enum BuyResult
    {
        Success,
        NotEnoughDiamonds
    }

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


        public static BuyResult ExecuteOrder(int idPerson, int idPack)
        {
            BuyResult result = BuyResult.Success;

            using (var db = new CardGame_v2Entities())
            {
                User user = db.AllUsers.FirstOrDefault(x => x.ID == idPerson);
                CardPack pack = db.AllCardPacks.FirstOrDefault(x => x.ID == idPack);

                if (user.AmountMoney < pack.PackPrice)
                {
                    result = BuyResult.NotEnoughDiamonds;
                    
                }
                else
                {
                    user.AmountMoney -= pack.PackPrice;

                    Random rnd = new Random();
                    int numberOfAllCards = db.AllCards.Count();

                    for (int i = 0; i < pack.NumCards; i++)
                    {
                        int rng = rnd.Next(1, numberOfAllCards);
                        var card = db.AllCards.OrderBy(x => x.ID).Skip(rng).Take(1).Single();

                        UserCardCollection coll = user.AllUserCardCollections.Where(x=>x.ID_Card == card.ID).FirstOrDefault();
                        //card 
                        if (coll != null) /// user hat diese karte schon einmal
                        {
                            coll.NumberOfCards++;
                        }
                        else
                        {
                            coll = new UserCardCollection()
                            {
                                ID_User = user.ID,
                                ID_Card = card.ID,
                                NumberOfCards = 1
                            };
                            db.AllUserCardCollections.Add(coll);
                        }
                    }
                    db.SaveChanges();
                }
            }
            return result;
        }
    }
}
