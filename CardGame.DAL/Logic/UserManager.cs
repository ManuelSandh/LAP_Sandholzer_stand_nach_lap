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
        public static List<tblUser> GetAllUser()
        {
            List<tblUser> ReturnList = null;
            using (var db = new CardGame_v2Entities())
            {
                // TODO - Include
                // .Include(t => t.tabelle) um einen Join zu machen !
                ReturnList = db.tblUser.ToList();
            }
            return ReturnList;
        }

        public static tblUser GetUserByEmail(string email)
        {
            tblUser dbUser = null;
            try
            {
                using (var db = new CardGame_v2Entities())
                {
                    dbUser = db.tblUser.Where(u => u.email == email).FirstOrDefault();
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
                    tblUser dbUser = db.tblUser.Where(u => u.email == email).FirstOrDefault();
                    if (dbUser == null)
                    {
                        throw new Exception("UserDoesNotExists");
                    }
                    role = dbUser.tblUserRole.rolename;
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
                tblUser dbUser = db.tblUser.Where(u => u.email == email).FirstOrDefault();

                gamerTag = dbUser.gamertag;

                return gamerTag;
            }

        }
        

        public static string GetRoleByEmail(string email)
        {
            string role = "";
            using (var db = new CardGame_v2Entities())
            {
                tblUser dbUser = db.tblUser.Where(u => u.email == email).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new Exception("UserDoesNotExist");
                }
                role = dbUser.tblUserRole.rolename;
            }
            return role;
        }
    }
}

        




