using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class AuthenticationUserModel
    {
        [Required(ErrorMessage =  "Email Adress is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage =  "Password is Required")]
        public string PassWord { get; set; }
    }
}