using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Register : Login
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihren Vornamen ein!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihren Nachnamen ein!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Erstellen Sie einen Gamertag!")]
        public string Gamertag { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein!")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage ="Bitte geben Sie Ihr Strasse an")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre Hausnummer an")]
        public int Streetnumber { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre PLZ an")]
        public string PLZ { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre Stadt an")]
        public string City { get; set; }
    }
}