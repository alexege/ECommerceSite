using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceSite.Models
{
    public class User
    {
        // auto-implemented properties need to match the columns in your table
        // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
        [Key]
        public int UserId { get; set; }
        // MySQL VARCHAR and TEXT types can be represeted by a string
        [Required(ErrorMessage="First Name is required.")]
        [MinLength(2, ErrorMessage="First Name must be at least 2 characters long.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage="Last Name is required.")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters long.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be as least 8 characters long.")]
        // [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[@#$%!]).+$", ErrorMessage="Password must contain at least 1 number, 1 letter, and 1 special character")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Confirm_Password { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }
    }
}
