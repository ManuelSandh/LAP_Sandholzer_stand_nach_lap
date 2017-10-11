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
    public class UserManager
    {
        public static List<User> GetAllUser()
        {
            List<User> ReturnList = null;
            using (var db = new CardGame_v2Entities())
            {
                // TODO - Include
                // .Include(t => t.tabelle) um einen Join zu machen !
                ReturnList = db.AllUsers.ToList();
            }
            return ReturnList;
        }

        public static User GetUserById(int id)
        {
            User dbUser = null;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbUser = db.AllUsers.Where(u => u.ID == id).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExists");
                    }
                }
            }
            catch (Exception e)
            {

                Log.Writer.LogError(e);
            }
            return dbUser;

        }

        public static User GetUserByEmail(string email)
        {
            User dbUser = null;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExists");
                    }
                }
            }
            catch (Exception e)
            {

                Log.Writer.LogError(e);
            }
            return dbUser;
        }

        public static string GetRoleNamesByEMail(string email)
        {
            string role = "";
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExists");
                    }
                    role = dbUser.UserRole.Name;
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }
            return role;
        }

        public static string getGamerTagByEmail(string email)
        {
            string gamerTag = "";

            using (var db = new CardGame_v2Entities())
            {
                User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();

                gamerTag = dbUser.GamerTag;

                return gamerTag;
            }
        }

        public static string GetRoleByEmail(string email)
        {
            string role = "";
            using (var db = new CardGame_v2Entities())
            {
                User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExist");
                }
                role = dbUser.UserRole.Name;
            }
            return role;
        }

        public static int GetNumDistinctCardsOwnedByEmail(string email)
        {
            int numCards = -1;
            using (var db = new CardGame_v2Entities())
            {
                User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExist");
                }
                numCards = dbUser.AllUserCardCollections.Count;
            }
            return numCards;
        }

        public static int GetNumTotalCardsOwnedByEmail(string email)
        {
            int numCards = -1;
            using (var db = new CardGame_v2Entities())
            {
                User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExist");
                }
                numCards = 0;
                foreach (var c in dbUser.AllUserCardCollections)
                {
                    numCards += c.NumberOfCards;
                }
            }
            return numCards;
        }

        public static int GetNumDecksOwnedByEmail(string email)
        {
            int numDecks = -1;
            using (var db = new CardGame_v2Entities())
            {
                User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExist");
                }
                numDecks = dbUser.AllDecks.Count;
            }
            return numDecks;
        }

       
        public static int GetBalanceByEmail(string email)
        {
            return GetUserByEmail(email).AmountMoney;
        }

        public static List<Card> GetAllCardsByEmail(string email)
        {
            var cardList = new List<Card>();

            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    var dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExist");
                    }
                    var dbCardCollection = dbUser.AllUserCardCollections.ToList();
                    if (dbCardCollection == null)
                    {
                        throw new Exception("CardCollectionNotFound");
                    }
                    foreach (var cc in dbCardCollection)
                    {
                        for (int i = 0; i < cc.NumberOfCards; i++)
                            cardList.Add(cc.Card);
                    }
                    return cardList;
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
                return null;
            }
        }

        public static bool UpdateBalanceByEmail(string email, int newBalance)
        {
            var dbUser = GetUserByEmail(email);

            dbUser.AmountMoney = newBalance;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    db.Entry(dbUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
                return false;
            }
        }

        public static bool AddCardsToCollectionByEmail(string email, List<Card> cards)
        {
            var dbUser = new User();
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExist");
                    }

                    foreach (var c in cards)
                    {
                        var userCC = (from coll in db.AllUserCardCollections
                                      where coll.ID_Card == c.ID && coll.ID_User == dbUser.ID
                                      select coll)
                                     .FirstOrDefault();

                        if (userCC == null) //User does not own card, add to collection
                        {

                            var cc = new UserCardCollection();
                            cc.Card = db.AllCards.Find(c.ID);
                            cc.User = dbUser;
                            cc.NumberOfCards = 1;
                            dbUser.AllUserCardCollections.Add(cc);
                            db.SaveChanges();
                        }
                        else //User owns card, add to num
                        {
                            userCC.NumberOfCards += 1;
                            db.Entry(userCC).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    //db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
                return false;
            }
        }

        public static List<Deck> GetAllDecksByEmail(string email)
        {
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    var dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExist");
                    }
                    var dbDecks = dbUser.AllDecks.ToList();
                    if (dbDecks == null)
                    {
                        throw new Exception("NoDecksFound");
                    }
                    return dbDecks;
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
                return null;
            }
        }

        public static void SaveUser(User dbUser)
        {
            using (var db = new CardGame_v2Entities())
            {
                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                
            }
        }


    }
}




