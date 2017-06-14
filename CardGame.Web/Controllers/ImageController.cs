using CardGame.DAL.Logic;
using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CardGame.Web.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        //[OutputCache(Duration = int.MaxValue, VaryByParam = "id")]
        //public ActionResult Card(int id = -1)
        //{

        //    ActionResult result = HttpNotFound();

        //    if (id > 0)
        //    {
        //        / get according pack from datastorage
        //        try
        //        {
        //            Card card = CardManager.GetCard(id);
        //            if (card != null)
        //            {
        //                result = File(card.Image, "image/jpg");
        //            }
        //        }
        //        catch (Exception)
        //        { }
        //    }

        //    return result;
        //}
    }
}