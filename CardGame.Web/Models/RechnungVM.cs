using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class RechnungVM
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }      
        public int IDPack { get; set; }
        public double Preis { get; set; }
        public string Pack { get; set; }
    }
}