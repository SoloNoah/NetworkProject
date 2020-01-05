using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proj.Models
{
    [Table("tblStudentCourses")]
    public class StudentCourses
    {
        [Required]
        [Key, Column(Order = 0)]
        public string CourseId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50")]
        public string Username { get; set; }

        [Column(Order = 2)]
        public int ExamA { get; set; }

        [Column(Order = 3)]
        public int ExamB { get; set; }
    }
}
