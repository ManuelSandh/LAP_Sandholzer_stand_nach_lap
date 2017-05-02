using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using CardGame.Log;
using CardGame.Web.Models;

namespace CardGame.Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<Register> UserList = new List<Register>();

            var dbUserlist = UserManager.GetAllUser();

            //var asd = new tblrole();
            

            foreach (var c in dbUserlist)
            {
                Register reguser = new Register();
                reguser.ID = c.ID;
                reguser.Firstname = c.FirstName;
                reguser.Lastname = c.LastName;
                reguser.Email = c.Mail;
                reguser.Role = c.UserRole.Name;
                reguser.Password = c.Password;
                reguser.Salt = c.UserSalt;

                UserList.Add(reguser);
            }
            return View(UserList);
        }

    }
}