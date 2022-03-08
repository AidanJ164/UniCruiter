using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Team5.Models;

namespace Team5.ViewModels
{
    public class StudentSearchViewModel
    {
        public List<Student> Students;
        public string SearchFirst;
        public string SearchLast;
    }
}
