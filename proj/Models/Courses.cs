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
        [Key, Column(Order = 0)]
        public string CourseId { get; set; }
        [Required]
        [Column(Order = 1)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Course name must be between 2 and 50")]
        public string CourseName { get; set; }
        [Required]
        [Column(Order = 2)]
        public string Day { get; set; }
        [Required]
        [Column(Order = 3)]
        public string Hour { get; set; }  
        [Required]
        [Column(Order = 4)]
        public string Room { get; set; }
       
    }
}