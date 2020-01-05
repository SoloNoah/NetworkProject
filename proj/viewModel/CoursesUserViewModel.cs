using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;

namespace proj.viewModel
{
    public class CoursesUserViewModel
    {
        public StudentCourses studentCourses { get; set; }
        public List<StudentCourses> courses { get; set; }
    }
}