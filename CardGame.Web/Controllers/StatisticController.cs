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
        [Authorize(Roles = "player")]
        public ActionResult Bestseller()
        {
            using (var cont = new CardGame_v2Entities())
            {
                            
                return View();
            }

        }


        [WebMethod]
        public static List<object> GetChartData()
        {
            string query = "Select Count(fkCardPack)";
            query += " from tblVirtualPurchase join tblCardPack on idCardPack = fkCardPack GROUP by fkCardPack desc";
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
        "fkCardPack"});
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
                        sdr["fkCardPack"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }
    }
}