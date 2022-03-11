using System;
using System.ComponentModel.DataAnnotations;

namespace UniCruiter.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string FirstName { get; set; }  

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Major { get; set; }

        [StringLength(6, MinimumLength = 4)]
        [Required]
        public string Season { get; set; }

        [Range(2022, 3000)]
        [Required]
        public int Year { get; set; }
    }
}
