using System;
using System.ComponentModel.DataAnnotations;

namespace Team5.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }
        public string Major { get; set; }

        [DataType(DataType.Date)]
        public DateTime GradDate { get; set; }
    }
}
