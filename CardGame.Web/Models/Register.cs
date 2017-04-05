using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Register : User
    {
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        public string confirmPassword { get; set; }
    }
}