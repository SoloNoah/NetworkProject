using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj.Models
{
    [Table("tblCourses")]
    public class Courses
    {
        [Required]
        [Key,Column(Order = 0)]
        public int CourseId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Course name must be between 2 and 15")]
        public string CourseName { get; set; }
    }
}
