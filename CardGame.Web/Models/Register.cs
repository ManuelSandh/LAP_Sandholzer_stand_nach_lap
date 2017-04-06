using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Register : Login
    {
        [Required(ErrorMessage = "Please enter your firstname!")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Please enter your lastname!")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Create a Gamertag!")]
        public string Gamertag { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match!")]
        public string confirmPassword { get; set; }
    }
}