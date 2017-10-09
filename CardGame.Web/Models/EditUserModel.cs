using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class EditUserModel
    {
        
        [MaxLength(50)]
        [DisplayName("Vorname")]
        public string Firstname { get; set; }
        
        [MaxLength(50)]
        [DisplayName("Nachname")]
        public string Lastname { get; set; }
        
        [MaxLength(20)]
        [DisplayName("Gamertag")]
        public string Gamertag { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }        

        [MaxLength(50)]
        [DisplayName("Strasse")]
        public string Street { get; set; }

        [MaxLength(15)]
        [DisplayName("Hausnummer")]
        public int Streetnumber { get; set; }
        
        [MaxLength(15)]
        [DisplayName("postleitzahl")]
        public string PLZ { get; set; }
        
        [MaxLength(30)]
        [DisplayName("Stadt")]
        public string City { get; set; }

    }
}