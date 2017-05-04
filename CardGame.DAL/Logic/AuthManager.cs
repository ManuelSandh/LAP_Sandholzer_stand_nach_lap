using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;

namespace CardGame.DAL.Logic
{
    public class AuthManager
    {
        public static bool Register(User regUser)
        {
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    if (db.AllUsers.Any(n => n.Mail == regUser.Mail))
                    {
                        throw new Exception("UserAlreadyExists");
                    }
                    //Salt erzeugen
                    string salt = Helper.GenerateSalt();

                    //Passwort Hashen
                    string hashedAndSaltedPassword = Helper.GenerateHash(regUser.Password + salt);

                    regUser.Password = hashedAndSaltedPassword;
                    regUser.UserSalt = salt;

                    db.AllUsers.Add(regUser);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);            
            }

            return true;
        }

        public static bool AuthUser(string email, string password)
        {
            try
            {
                string dbUserPassword = null;
                string dbUserSalt = null;

                using (var db = new CardGame_v2Entities())
                {
                    User dbUser = db.AllUsers.Where(u => u.Mail == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExist");
                    }

                    dbUserPassword = dbUser.Password;
                    dbUserSalt = dbUser.UserSalt;

                    Log.Writer.LogInfo("Entered Pass = " + password);

                    password = Helper.GenerateHash(password + dbUserSalt);

                    Log.Writer.LogInfo("HashPass = " + password);

                    if (dbUserPassword == password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
                return false;
            }
        }
    }
}

//            