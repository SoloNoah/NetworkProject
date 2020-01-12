using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;

namespace proj.viewModel
{
    public class LecturerViewModel
    {
        public Lecturer lecturer { get; set; }
        public List<Lecturer> lecturerList { get; set; }
    }
}