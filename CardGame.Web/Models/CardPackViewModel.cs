using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class CardPackViewModel
    {
        public int IDCardPack { get; set; }
        public string PacketName { get; set; }
        public int Anzahlkarten { get; set; }
        public double Preis { get; set; }
        public bool Aktiv { get; set; }
    }
}