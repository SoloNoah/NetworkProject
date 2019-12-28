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
        public List<Courses> courses { get; set; }
    }
}