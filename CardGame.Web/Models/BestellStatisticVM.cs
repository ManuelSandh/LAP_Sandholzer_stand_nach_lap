using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class BestellStatisticVM
    {
        public List<int> Jahre { get; set; }
        public List<int> Monate { get; set; }
        public List<decimal> Preis { get; set; }

        public int AusgewJahr { get; set; }

        public string Chart { get; set; }

        public BestellStatisticVM()
        {
            Jahre = new List<int>();
          
            AusgewJahr = DateTime.Now.Year;
        }

    }
}