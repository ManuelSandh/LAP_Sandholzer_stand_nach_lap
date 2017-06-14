using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using CardGame.Web.Models;
using System.Web.Security;
using System.Data.Entity;

namespace CardGame.Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<Register> UserList = new List<Register>();
            var dbUserlist = UserManager.GetAllUser();

            foreach (var c in dbUserlist)
            {
                Register reguser = new Register();
                reguser.ID = c.ID;
                reguser.Firstname = c.FirstName;
                reguser.Lastname = c.LastName;
                reguser.Email = c.Mail;
                reguser.Role = c.UserRole.Name;
                reguser.Gamertag = c.GamerTag;
                //reguser.Street = c.Street;
                //reguser.Streetnumber = (int)c.Streetnumber;
                //reguser.PLZ = c.Post_Code;
                //reguser.City = c.City;

                UserList.Add(reguser);
            }
            return View(UserList);
        }

        public ActionResult EditAccount()
        {
            Register user = new Register();
            user.Firstname = "BLA";
            return View(user);
        }

        [HttpPost]
        public ActionResult EditAccount(Register user)
        {
            var reguser = UserManager.GetUserByEmail(User.Identity.Name);
            reguser.ID = user.ID;
            reguser.FirstName = user.Firstname;
            reguser.LastName = user.Lastname;
            reguser.Mail = user.Email;
            reguser.GamerTag = user.Gamertag;
            //reguser.Street = user.Street;
            //reguser.Streetnumber = (int)user.Streetnumber;
            //reguser.Post_Code = user.PLZ;
            //reguser.City = user.City;

            using (var db = new CardGame_v2Entities())
            {
                db.Entry(reguser);
                db.SaveChanges();

                return View(reguser);
            }
        }
    }
}



