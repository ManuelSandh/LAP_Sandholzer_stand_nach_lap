using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using CardGame.Web.Models;

namespace CardGame.Web.Controllers
{
    public class LAPController : Controller
    {
        [Authorize(Roles = "player, admin")]
        [HttpGet]
        public ActionResult BestellStatistik()
        {

            BestellStatisticVM bsvm = new BestellStatisticVM();
            int aktuellesJahr = DateTime.Now.Year;

            
          var allOrders = DAL.ExtensionManager.GetAllOdersFromYear(User.Identity.Name, bsvm.AusgewJahr);

            var groupOrders = allOrders
                .GroupBy(o => o.PurchaseDate.Month)
                .Select(s => new
            {
                    //TODO .Month auf gesamtpreis ändern - das Feld habe ich noch nicht in der DB
                Month = s.Key,
                MonthSum = s.Sum(o => o.PurchaseDate.Month)
            }); 




            var myChart = new Chart(width: 600, height: 400)
                .AddTitle("Chart Title")
                .AddSeries(
                name: "Employeee",
                xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                yValues: new[] { "2", "6", "4", "5", "3" })
                .GetBytes();

            bsvm.Chart = Convert.ToBase64String(myChart);
            bsvm.Chart = String.Format("data:image/png;base64, {0}", bsvm.Chart);
            bsvm.Jahre = DAL.ExtensionManager.GetYearsOrdered(User.Identity.Name);

            return View(bsvm);
        }
    }
}