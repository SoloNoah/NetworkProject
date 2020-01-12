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
        public string CourseName { get; set; }

        [Required]
        [Column(Order = 2)]
        public string Day { get; set; }

        [Required]
        [Column(Order = 3)]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan SHour { get; set; }  

        [Required]
        [Column(Order = 4)]
  
        public string Room { get; set; }

        [Required]
        [Column(Order = 5)]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan EHour { get; set; }

    }
}