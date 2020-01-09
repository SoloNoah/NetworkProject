using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;
namespace proj.viewModel
{
    public class faMemberViewModel
    {
        public List<Student> faStudents { get; set; }
        public List<Courses> faCourses { get; set; }
    }
}