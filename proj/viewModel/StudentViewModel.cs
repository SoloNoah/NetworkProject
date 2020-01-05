using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;

namespace proj.viewModel
{
    public class StudentViewModel
    {
        public Student student { get; set; }
        public List<Student> studentList { get; set; }
    }
}