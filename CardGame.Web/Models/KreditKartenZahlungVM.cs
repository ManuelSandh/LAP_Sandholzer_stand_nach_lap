using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class KreditKartenZahlungVM
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string KartenNr { get; set; }
        public string CVV { get; set; }
        public DateTime AblaufDatum { get; set; }

        public double Preis { get; set; }
        public int AnzahlDiamanten { get; set; }
        public int IdDiamantenPack { get; set; }
    }
}