using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLayer
{
    public class User
    {
        //Getter and Setters 
        //Validates that this is the primary key 
        [Key]
        public int UserID { get; set; }


        //Validates that is required and message if not met
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your last name")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Gender?")]
        [Required(ErrorMessage = "Plase enter your gender.")]
        public string Gender { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "State?")]
        [Required(ErrorMessage = "Plase enter your state.")]
        public string State { get; set; }

        [Display(Name = "Age")]
        [Required(ErrorMessage = "Please enter an age")]
        public int Age { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "EmailAddress")]
        [Required(ErrorMessage = "Please enter an email address")]
        public string Email { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter a username")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Please enter a password")]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Please enter a confirm password")]
        [Required(ErrorMessage = "Please enter a confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


    }
}
