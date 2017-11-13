using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.DAL.Model;
using CardGame.Log;
using System.Data.Entity;


namespace CardGame.DAL
{
   public class ExtensionManager
    {
        public static List<int> GetYearsOrdered(string email)
        {
            using (var db = new CardGame_v2Entities())
            {
                var dbBenutzer = db.AllUsers
                    .Include(u => u.AllVirtualPurchase)
                    .Where(b => b.Mail == email)
                    .FirstOrDefault();

                var dbBestellungen = dbBenutzer.AllVirtualPurchase.ToList();

                var jahre = dbBestellungen.Distinct()
                    .Where(b => b.PurchaseDate != null)
                    .Select(b => b.PurchaseDate.Year)
                    .ToList();

                return jahre;
            }
            
        }
        public static List<VirtualPurchase> GetAllOdersFromYear(string email, int year)
        {
            using (var db = new CardGame_v2Entities())
            {
                var dbOrders = db.AllVirtualPurchases                   
                    .Where(b => b.User.Mail == email && b.PurchaseDate.Year == year)
                    .ToList();

                return dbOrders;
            }
        }


    }
}
