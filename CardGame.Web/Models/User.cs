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
        public string Salt { get; set; }
        public string Role { get; set; }
    }
}