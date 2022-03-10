using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using UniCruiter.Models;

namespace UniCruiter.ViewModels
{
    public class StudentSearchViewModel
    {
        public List<Student> Students;
        public SelectList Majors;
        public SelectList Years;
        public SelectList Seasons;
        public string Season;
        public int Year;
        public string Major;
        public string SearchFirst;
        public string SearchLast;
    }
}
