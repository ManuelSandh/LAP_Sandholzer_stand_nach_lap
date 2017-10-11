﻿using System;
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

                    User newUser = new User()
                    {
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
                        AllDecks = decks
                       
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

        public static bool AuthUser(string email, string password)
        {
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    User user = db.AllUsers.FirstOrDefault(u => u.Mail == email);
                    if (user == null)
                        throw new Exception("UserDoesNotExist");
                    
                    return user.Password.SequenceEqual(Helper.GenerateHash(password + user.UserSalt));
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