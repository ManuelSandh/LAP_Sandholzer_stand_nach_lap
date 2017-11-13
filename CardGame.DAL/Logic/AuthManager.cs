using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Net.Mail;
using System.Net;

namespace CardGame.DAL.Logic
{
    public class AuthManager
    {
        public static bool Register(string mail, string password, string firstName, string lastName, string gamerTag, string street, int streetnumber, string PLZ, string city)
        {
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    if (db.AllUsers.Any(n => n.Mail == mail))
                    {
                        throw new Exception("UserAlreadyExists");
                    }
                    //Salt erzeugen
                    string salt = Helper.GenerateSalt();

                    //Passwort Hashen
                    byte[] hashedAndSaltedPassword = Helper.GenerateHash(password + salt);

                    List<Deck> decks = new List<Deck>();
                    

                    for (int i = 1; i <= 3; i++)
                    {
                        Deck d = new Deck();
                        d.Name = (gamerTag +"'s" + " Deck " + i).ToString();
                        decks.Add(d);
                    }

                    Guid guid = Guid.NewGuid();
                    //TODO Aufruf von Aktivierungslink - Einkommentieren wenn gebraucht
                    //SendActivationCodeToUser(mail, guid.ToString());

                    User newUser = new User()
                    {
                        //Standart werte an den neu registrierten User übergeben
                        Password = hashedAndSaltedPassword,
                        UserSalt = salt,
                        FirstName = firstName,
                        LastName = lastName,
                        Mail = mail,
                        GamerTag = gamerTag,
                        Street = street,
                        Streetnumber = streetnumber,
                        Post_Code = PLZ,
                        City = city,
                        AmountMoney = 300,
                        ID_UserRole = 2,
                        AllDecks = decks,
                        Active = false,
                        activationCode = guid // Hier wird der Aktivierungscode erzeugt
                       
                    };
                    db.AllUsers.Add(newUser);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Writer.LogError(e);
            }

            return true;
        }
        //TODO Methode für Aktivierungslink
        //private static void SendActivationCodeToUser(string email, string guid)
        //{
        //    string host = "smtp.gmail.com";
        //    int port = 587;
        //    string betreff = "Ihr Clonestone Aktivierungscode";
        //    string nachricht = "Bitte klicken Sie auf folgenden Link um Ihren" +
        //        "Clonestone Account zu Aktivieren";
        //    string clonestoneURL = "http://localhost:56538";


        //    string link = "<a href='"
        //        + clonestoneURL
        //        + "Account/Activate/" 
        //        + guid 
        //        + "'>Hier klicken!</a>'";

        //    string sender = "htmanus23@gmail.com";


        //    SmtpClient smtp = new SmtpClient();

        //    MailMessage mm = new MailMessage(sender, email);          
          
        //    mm.Body = nachricht + link; //Nachricht der Emal
        //    mm.Subject = betreff; //Betreff der Email

        //    NetworkCredential nc = new NetworkCredential(sender, "123user!");
        //    //Login und Passwort füer SMTP Server in ctor

        //    smtp.Host = host;
        //    smtp.Port = port;
        //    smtp.UseDefaultCredentials = true;
        //    smtp.Credentials = nc;
        //    smtp.EnableSsl = true;

        //    smtp.Send(mm);

        //}


        public static bool AuthUser(string email, string password)
        {
            bool ergebnis = false;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    User user = db.AllUsers.FirstOrDefault(u => u.Mail == email);
                    if (user == null)
                        throw new Exception("UserDoesNotExist");
                    
                     var pwGleich = user.Password.SequenceEqual(Helper.GenerateHash(password + user.UserSalt));
                    if (user.Active == true && pwGleich == true)
                        ergebnis = true;
                    return ergebnis;
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
         