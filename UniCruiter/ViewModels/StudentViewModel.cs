using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniCruiter.Models;
using UniCruiter.Models.Identity;


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
                Comments = student.Comments;
            }
        }

        public int Id { get; set; }
        
        [Display(Name = "First Name:")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string FirstName { get; set; }  

        [Display(Name = "Last Name:")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Name:")]
        public string FullName => FirstName + " " + LastName;

        [Display(Name = "Major:")]
        [StringLength(50, MinimumLength = 2)]
        [Required]
        public string Major { get; set; }

        [Display(Name = "Graduation Season:")]
        [StringLength(6, MinimumLength = 4)]
        [Required]
        public string Season { get; set; }

        [Display(Name = "Graduation Year:")]
        [Range(2022, 3000)]
        [Required]
        public int Year { get; set; }

        [Display(Name = "Graduation:")]
        public string Graduation => Season + " " + Year;


        [Display(Name = "Email Address:")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Comment")]
        public string CommentText { get; set; }

        [Display(Name = "Date")]
        public DateTime CommentEnteredOn { get; set; }

        [Display(Name = "User")]
        public ApplicationUser CommentEnteredBy { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public SelectList Majors;
        public SelectList Years;
        public SelectList Seasons;
        

    }
}
