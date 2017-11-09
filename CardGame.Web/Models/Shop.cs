using CardGame.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Shop : User
    {
        public List<CardPackModel> cardPacks { get; set; }
        public List<DiamantenModel> diamantPacks { get; set; }
       
    }
}