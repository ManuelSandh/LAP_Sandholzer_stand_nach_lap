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
                UserList.Add(reguser);
            }
            return View(UserList);
        }

        public ActionResult Edit()
        {
            DAL.Model.User user = UserManager.GetUserByEmail(User.Identity.Name);
            
            return View();
        }
    }
}




