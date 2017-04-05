using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardGame.Web.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter your firstname!")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Please enter your lastname!")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Please enter your Email!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Create a Gamertag!")]
        public string Gamertag { get; set; }
        [Required(ErrorMessage = "Please enter your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}