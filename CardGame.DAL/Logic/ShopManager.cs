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
    public class ShopManager
    {
        public static List<CardPack> getAllCardPacks()
        { 
            using (CardGame_v2Entities cont = new CardGame_v2Entities())
            {
                return cont.tblCardPack.ToList();
            }           
        }
    }
}    