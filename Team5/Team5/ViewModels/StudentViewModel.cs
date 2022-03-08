using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using UniCruiter.Models;

namespace UniCruiter.ViewModels
{
    public class StudentSearchViewModel
    {
        public List<Student> Students;
        public string SearchFirst;
        public string SearchLast;
    }
}
