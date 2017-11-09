using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.Web.Models;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Web.Security;
using System.Data.Entity;

namespace CardGame.Web.Controllers
{
    public class AccountController : Controller
    {
        //GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //TempData["ErrorMessage"] = "Fehlermeldungstext";
        //TempData["ConfirmMessage"] = "Bestätigungstext";

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            else
            {
                login.Role = UserManager.GetRoleByEmail(login.Email);
                bool hasAccess = AuthManager.AuthUser(login.Email, login.Password);

                //Authentifizierung
                if (!hasAccess)
                {
                    TempData["ErrorMessage"] = "Email oder Passwort falsch!";
                    return View(login);
                }
                else
                {
                    try
                    {
                        var authTicket = new FormsAuthenticationTicket(
                                        1,                              //Ticketversion
                                        login.Email,                    //UserIdentifizierung
                                        DateTime.Now,                   //Zeitpunkt der Erstellung
                                        DateTime.Now.AddMinutes(20),    //Gültigkeitsdauer
                                        true,                           //Persistentes Ticket über Sessions hinweg
                                        login.Role            //Userrolle(n)
                                        );

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    }
                    catch (Exception e)
                    {
                        Writer.LogError(e);
                    }
                }
            }

            string rolle = UserManager.GetRoleNamesByEMail(login.Email);
            AuthManager.AuthUser(login.Email, login.Password);
            string gamertag = UserManager.getGamerTagByEmail(login.Email);

            TempData["ConfirmMessage"] = "Willkomen" + " " + gamertag;

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["ConfirmMessage"] = "Erfolgreich ausgeloggt";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isRegistered = AuthManager.Register(model.Email, model.Password, model.Firstname, model.Lastname, model.Gamertag, model.Street, model.Streetnumber, model.PLZ, model.City);

            //Authentifizierung
            if (!isRegistered)
            {
                return View(model);
            }

            TempData["ConfirmMessage"] = "Erfolgreich registriert";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UserProfile()
        {
            DAL.Model.User dbuser = DAL.Logic.UserManager.GetUserByEmail(User.Identity.Name);
            Web.Models.EditUserModel vmUser = new Models.EditUserModel();

            vmUser.Firstname = dbuser.FirstName;
            vmUser.Lastname = dbuser.LastName;
            vmUser.Gamertag = dbuser.GamerTag;
            vmUser.Street = dbuser.Street;
            vmUser.Streetnumber = dbuser.Streetnumber ?? -1;
            vmUser.PLZ = dbuser.Post_Code;
            vmUser.City = dbuser.City;

            return View(vmUser);
        }


        [HttpGet]
        public ActionResult EditUser()
        {
            DAL.Model.User dbuser = DAL.Logic.UserManager.GetUserByEmail(User.Identity.Name);
            Web.Models.EditUserModel vmUser = new Models.EditUserModel();

            vmUser.Firstname = dbuser.FirstName;
            vmUser.Lastname = dbuser.LastName;
            vmUser.Gamertag = dbuser.GamerTag;
            vmUser.Street = dbuser.Street;
            vmUser.Streetnumber = dbuser.Streetnumber ?? -1;
            vmUser.PLZ = dbuser.Post_Code;
            vmUser.City = dbuser.City;

            return View(vmUser);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserModel EM)
        {
            DAL.Model.User dbUser = DAL.Logic.UserManager.GetUserByEmail(User.Identity.Name);

            dbUser.FirstName = EM.Firstname;
            dbUser.LastName = EM.Lastname;
            dbUser.GamerTag = EM.Gamertag;
            dbUser.Street = EM.Street;
            dbUser.Streetnumber = EM.Streetnumber;
            dbUser.Post_Code = EM.PLZ;
            dbUser.City = EM.City;

            DAL.Logic.UserManager.SaveUser(dbUser);

            return RedirectToAction("UserProfile");
        }

        [HttpGet]
        public ActionResult ActivateUser(string id)
        {
            try
            {
                //1. In DB nach entsprechenden String suchen
                //2. Wenn gefunden, den zugehörigen Benutzer aktivieren,
                //3. GUID aus DB loeschen
                using (var db = new CardGame_v2Entities())
                {
                    //1
                    var dbUser = db.AllUsers.Where(u => u.activationCode.ToString() == id).FirstOrDefault();

                    if (dbUser == null)
                        return RedirectToAction("Index", "Home");
                    //2
                    dbUser.Active = true;
                    //3
                    dbUser.activationCode = null;
                    //4
                    db.Entry(dbUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login");
        }
       
        [HttpGet]
        [Authorize(Roles = "player")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "player")]
        public ActionResult ChangePassword(ChangePasswordModel neuesPasswortDaten)
        {
            byte[] dbHashPasswort = UserManager.getCurrentPassword(User.Identity.Name);
            CardGame.DAL.Model.User dbUser = UserManager.GetUserByEmail(User.Identity.Name);

            
            byte[] altesOberflächeHashPasswort = Helper.GenerateHash(neuesPasswortDaten.CurrentPassword + dbUser.UserSalt);

            byte[] neuesOberflächenHashPasswort = Helper.GenerateHash(neuesPasswortDaten.NewPassword + dbUser.UserSalt);

            if(dbHashPasswort.SequenceEqual(altesOberflächeHashPasswort) == true)
            {
                //Passwörter sind gleich
                if(ModelState.IsValid)
                {
                    //Validierung ist ok - deswegen sind passwort und passwort wiederholen gleich
                    //setze das neue Passwort
                    UserManager.setNewPasswort(User.Identity.Name, neuesOberflächenHashPasswort);
                }
            }

            return View();
        }

    }
}