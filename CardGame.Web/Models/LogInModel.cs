using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Email muss angegeben werden")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kennwort muss angegeben werden")]
        [DataType(DataType.Password)]
        public string Passwort { get; set; }
    }
}