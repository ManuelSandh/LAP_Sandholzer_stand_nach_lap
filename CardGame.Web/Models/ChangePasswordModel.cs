using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class ChangePasswordModel
    {
        public string Email { get; set; }

        [Required, Display(Name = "Aktuelles Password")]
        public string CurrentPassword { get; set; }

        [Required, Display(Name = "Neues Passwort")]
        public string NewPassword { get; set; }

        [Compare("NewPassword"), Display(Name = "Passwort wiederholen")]
        public string ConfirmPassword { get; set; }

    }
}