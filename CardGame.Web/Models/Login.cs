using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Login : User
    {
        [Required(ErrorMessage = "Bitte geben Sie Ihre Email Adresse ein")]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihr Passwort ein!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}