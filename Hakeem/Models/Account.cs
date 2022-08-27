using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hakeem.Models
{
    public class Account
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Enter Full Name")]
        [Display(Name = "Full Name")]
        [StringLength(50, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter a valid Name")]
        public string Name { get; set; }

        [StringLength(40, ErrorMessage = "Email Lenght Range exceed Must Enter 50 Character")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter Email Address ")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [MaxLength(10)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Select User Type")]
        [Display(Name = "User Type")]
        public string Type { get; set; }

        public int count { get; set; }
        public decimal Stars { get; set; }
    }
   
}
public enum type { Patient,Hakeem}