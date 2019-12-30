using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;

namespace proj.viewModel
{
    public class CoursesUserViewModel
    {
        public CourseUser course { get; set; }
        public List<CourseUser> courses { get; set; }
    }
}