using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer
{
    /// <summary>
    /// Used for login to save attempt and 
    /// check if login is valid
    /// </summary>
    public class Authentication
    {

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }
       

        [DataType(DataType.Password)]
        [Display(Name = "Please enter a password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        public int UserID { get; set; } = 1;
    }
}
