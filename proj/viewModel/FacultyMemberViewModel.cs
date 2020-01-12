using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;
namespace proj.viewModel
{
    public class FacultyMemberViewModel
    {
        public List<Student> faStudents { get; set; }
        public List<Courses> faCourses { get; set; }
        public List<Lecturer> faLecturers { get; set; }
        public List<int> faLecturersID { get; set; }
    }
}