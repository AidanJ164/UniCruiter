using System;
using System.ComponentModel.DataAnnotations;

namespace UniCruiter.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public string Major { get; set; }
        public string Season { get; set; }
        public string Graduation => Season + " " + Year;
        public int Year { get; set; }
        public string Email { get; set; }
    }
}
