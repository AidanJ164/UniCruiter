using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniCruiter.Models;

namespace UniCruiter.ViewModels
{
    public class StudentViewModel
    {
        public StudentViewModel()
        {

        }

        public StudentViewModel( Student student )
        {
            if(student != null)
            {
                Id = student.Id;
                FirstName = student.FirstName;
                LastName = student.LastName;
                Major = student.Major;
                Season = student.Season;
                Year = student.Year;
                Email = student.Email;
            }
        }

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

        [Required]
        public string Email { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public SelectList Majors;
        public SelectList Years;
        public SelectList Seasons;
    }
}
