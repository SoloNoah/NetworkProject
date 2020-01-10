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
        [RegularExpression("^[0-9]{1}$", ErrorMessage = "Course Id must be at least 1 digits")]
        public string CourseId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Course name must be between 4 and 50")]
        public string CourseName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Day must be between 6 and 50")]
        public string Day { get; set; }
        [Required]
        public TimeSpan SHour { get; set; }  
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "room must be between 3 and 50")]
        public string Room { get; set; }
        [Required]
        public TimeSpan EHour { get; set; }

    }
}