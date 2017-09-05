using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace CardGame.Web.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        [HttpGet]      
        public ActionResult Bestseller()
        {            
                return View();           
        }

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            string query = "Select Top 3 Count(packname) as amount, packname ";
            query += " from tblVirtualPurchase join tblCardPack on idCardPack = fkCardPack GROUP by packname order by amount desc";
            string constr = "Data Source=PC001999F9405B;" + "initial catalog=CardGame_v2;" + "user id=sa;password=123user!";
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
        "packname", "amount"});
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                        sdr["packname"] , sdr["amount"]
                            });
                        }
                    }
                    con.Close();

                }
            }
            return Json(chartData);

        }
    }
}