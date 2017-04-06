using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class Login : User
    {
        [Required(ErrorMessage = "Please enter your Email!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}