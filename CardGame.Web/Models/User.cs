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
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email muss angegeben werden")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bitte geben Sie einen Gamertag an")]
        public string Gamertag { get; set; }

        [Required(ErrorMessage = "Kennwort muss angegeben werden")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}