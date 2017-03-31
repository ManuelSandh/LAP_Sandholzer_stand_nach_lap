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
        // GET: Account
        [HttpGet]
        public ActionResult _Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Login(User login)
        {
            bool hasAccess = AuthManager.AuthUser(login.Email, login.Password);
            login.Role = UserManager.GetRoleNamesByEMail(login.Email);

            if (hasAccess && login.Role != "")
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
            else
            {
                return View(login);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult _Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Register(User regUser)
        {
            var dbUser = new tblperson();

            dbUser.firstname = regUser.Firstname;
            dbUser.lastname = regUser.Lastname;

            dbUser.email = regUser.Email;
            dbUser.password = regUser.Password;
            dbUser.salt = regUser.Salt;
            dbUser.userrole = "player";
            dbUser.currencybalance = 100;

            //dbUser.tblrole = new List<tblrole>();
            //dbUser.tblrole.Add(new tblrole());
            //dbUser.tblrole.FirstOrDefault().rolename = "user";

            AuthManager.Register(dbUser);
             

           return RedirectToAction("_Login");
           
          
        }
    }
}