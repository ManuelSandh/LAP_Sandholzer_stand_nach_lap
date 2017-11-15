using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class KreditKartenZahlungVM
    {
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        [Required]        
        public string KartenNr { get; set; }
        [Required]
        public string CVV { get; set; } 

        public DateTime? AblaufDatum { get; set; }

        public double Preis { get; set; }
        public int AnzahlDiamanten { get; set; }
        public int IdDiamantenPack { get; set; }
    }
}