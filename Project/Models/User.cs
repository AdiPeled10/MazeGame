using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class User
    {
        [Required]
        public string UserName { get; set; }
        [Key]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

    }
}