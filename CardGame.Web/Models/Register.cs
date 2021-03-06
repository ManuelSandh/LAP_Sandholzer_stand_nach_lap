﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Register : Login
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihren Vornamen ein!")]
        [MaxLength(50)]
        [DisplayName("Vorname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihren Nachnamen ein!")]
        [MaxLength(50)]
        [DisplayName("Nachname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Erstellen Sie einen Gamertag!")]
        [MaxLength(20)]
        [DisplayName("Gamertag")]
        public string Gamertag { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein!")]
        [DisplayName("Passwort wiederholen")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihr Strasse an")]
        [MaxLength(50)]
        [DisplayName("Straße")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre Hausnummer an")]
        [DisplayName("Hausnummer")]
        public int Streetnumber { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre PLZ an")]
        [DisplayName("PLZ")]
        public string PLZ { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre Stadt an")]
        [MaxLength(30)]
        [DisplayName("Stadt")]
        public string City { get; set; }
    }
}