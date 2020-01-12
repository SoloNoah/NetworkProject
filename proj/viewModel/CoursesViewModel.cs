using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;

namespace proj.viewModel
{
    public class CoursesViewModel
    {
        public Courses course { get; set; }
        public Exams exam { get; set; }
        public List<Courses> courses { get; set; }
        public List<Exams> exams { get; set; }
        public List<string> examsId { get; set; }
    }
}