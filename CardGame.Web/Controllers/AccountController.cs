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

        //TODO : wenn das Passwort falsch ist Fehlermeldung



        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                //TempData["ErrorMessage"] = "Fehlermeldungstext";
                //TempData["ConfirmMessage"] = "Bestätigungstext";
                return View(login);
            }
            else
            {
                bool hasAccess = AuthManager.AuthUser(login.Email, login.Password);
         
                //Authentifizierung
                if (!hasAccess)
                {
                    return View(login);
                }
            }

            string rolle = UserManager.GetRoleNamesByEMail(login.Email);
            auth(login.Email, login.Password, rolle);

            
            TempData["ConfirmMessage"] = "Erfolgreich eingeloggt";

            return RedirectToAction("Index", "Home");
        }

        void auth(string email, string passwort, string rolle)
        {  
            var authTicket = new FormsAuthenticationTicket(
                            1,                              //Ticketversion
                            email,                    //UserIdentifizierung
                            DateTime.Now,                   //Zeitpunkt der Erstellung
                            DateTime.Now.AddMinutes(20),    //Gültigkeitsdauer
                            true,                           //Persistentes Ticket über Sessions hinweg
                            rolle            //Userrolle(n)
                            );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register regUser)
        {   
            if (!ModelState.IsValid)
            {
                return View(regUser);
            }

            var dbUser = new tblperson();

            dbUser.firstname = regUser.Firstname;
            dbUser.lastname = regUser.Lastname;

            dbUser.email = regUser.Email;
            dbUser.password = regUser.Password;
            dbUser.salt = regUser.Salt;
            dbUser.userrole = "user";
            dbUser.currencybalance = 100;

            //dbUser.tblrole = new List<tblrole>();
            //dbUser.tblrole.Add(new tblrole());
            //dbUser.tblrole.FirstOrDefault().rolename = "user";

            bool isRegistered = AuthManager.Register(dbUser);
            
            //Authentifizierung
            if (!isRegistered)
            {              
                
                return View(regUser);
            }

            auth(dbUser.email, dbUser.password, dbUser.userrole);
            
            TempData["ConfirmMessage"] = "Register finished";
            return RedirectToAction("Index", "Home");
        }
    }
}