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
        NotEnoughDiamonds,
        NotEnoughMoney
    }

    public class ShopManager
    {
        public static List<CardPack> AlleMeineCardpacks()
        {
            using (CardGame_v2Entities db = new CardGame_v2Entities())
            {
                return db.AllCardPacks.ToList();
            }
        }

        public static CardPack LiefereCardPack(int? id)
        {
            using (CardGame_v2Entities db = new CardGame_v2Entities())
            {
                return db.AllCardPacks.Find(id);
            }
        }


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

        public static List<CardPack> getAllAktiveCardPacks()
        {
            var allCardPacks = new List<CardPack>();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    allCardPacks = db.AllCardPacks.Where(x=>x.Aktiv==true).ToList();
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
                VirtualPurchase order = new VirtualPurchase();
                order.ID_CardPack = idPack;
                order.ID_User = idPerson;
                order.PurchaseDate = DateTime.Now;
                order.NumberOfPacks = 1;
                db.AllVirtualPurchases.Add(order);
                db.SaveChanges();


                /// ermittle User und Pack für übergebene IDs
                User user = db.AllUsers.FirstOrDefault(x => x.ID == idPerson);
                CardPack pack = db.AllCardPacks.FirstOrDefault(x => x.ID == idPack);

                // prüfe auf ungültige Daten
                if (user == null)
                    throw new ArgumentException("Ungültige idPerson");
                if (pack == null)
                    throw new ArgumentException("Ungültige idPack");

                /// prüfe ob user genügend IngameGeld hat!
                if (user.AmountMoney < pack.PackPrice)
                {
                    result = BuyResult.NotEnoughDiamonds;
                }
                else
                {
                    /// ziehe Preis vom pack beim User ab!
                    user.AmountMoney -= pack.PackPrice;

                    /// Ermittle die Karten die der User (im Pack) jetzt gekauft hat!
                    Random rnd = new Random();
                    int numberOfAllCards = db.AllCards.Count();

                    for (int i = 0; i < pack.NumCards; i++)
                    {
                        /// ermittle einen zufälligen Index einer möglichen Karte
                        int rng = rnd.Next(0, numberOfAllCards);

                        /// überspringe alle karten VOR diesem Index (daher auch: rng-1)
                        /// und nimm danach die nächste Karte
                        var card = db.AllCards.OrderBy(x => x.ID).Skip(rng - 1).Take(1).Single();

                        /// ermittle nun ob der User DIESE Karte schon einmal hat
                        UserCardCollection coll = user.AllUserCardCollections.Where(x => x.ID_Card == card.ID).FirstOrDefault();

                        //card 
                        if (coll != null) /// user hat diese karte schon einmal
                        {
                            coll.NumberOfCards++;
                        }
                        else /// user hat Karte noch nicht
                        {
                            /// also füge sie genau 1x hinzu
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

        public static BuyResult ExecuteDiamantenOrder(int idPerson, int idDiaPack)
        {
            BuyResult result = BuyResult.Success;

            using (var db = new CardGame_v2Entities())
            {
                VirtualPurchase diaOrder = new VirtualPurchase();

                diaOrder.ID_DiamantenPack = idDiaPack;
                diaOrder.ID_User = idPerson;
                diaOrder.PurchaseDate = DateTime.Now;
                diaOrder.NumberOfPacks = 1;
                db.AllVirtualPurchases.Add(diaOrder);
                db.SaveChanges();


                User user = db.AllUsers.FirstOrDefault(x => x.ID == idPerson);
                DiamantenPack pack = db.AllDiamantenPacks.FirstOrDefault(x => x.ID == idDiaPack);

                if (user.AmountMoney < pack.PackPrice)
                    result = BuyResult.NotEnoughMoney;
                else
                {
                    /// ziehe Preis vom pack beim User ab!
                    user.AmountMoney += (int)pack.Diamanten;
                    db.SaveChanges();
                }

            }
            return result;
        }
        public static void saveRatinginDB(int ratingSubmit, int? star)
        {
            UserRanking userranking = new UserRanking();
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    userranking.pack_id = ratingSubmit;
                    userranking.rating = (short)star;


                    db.UserRanking.Add(userranking);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static double GetPackRatingAverageById(int id)
        {
            double ratingResult = 0;
            double countedRating = 0;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    var result = db.UserRanking.Where(u => u.pack_id == id).ToList();

                    double number = result.Count();

                    foreach (var item in result)
                    {
                        countedRating = countedRating + item.rating ?? -1;
                    }
                    ratingResult = countedRating / number;
                    ratingResult = Math.Round(ratingResult, 1);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return ratingResult;
        }

        public static void UpdateCardPackAktiv(int idCardpack, bool? aktiv)
        {
            using (var db = new CardGame_v2Entities())
            {
                //suchen cardpack anhand der id
                CardPack gefundenesPaket = db.AllCardPacks.Find(idCardpack);

                gefundenesPaket.Aktiv = aktiv;

                //db.Entry(gefundenesPaket).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

    }
}
