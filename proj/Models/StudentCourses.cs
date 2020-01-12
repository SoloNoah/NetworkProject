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
        [Range(0,6, ErrorMessage = "Choose between 1 and 6 included.")]
        public string CourseId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50")]
        public string Username { get; set; }
        [Required]
        [Column(Order = 2)]
        [Range(0,100, ErrorMessage = "Grade must be between 0-100.")]
        public int ExamA { get; set; }
        [Required]
        [Column(Order = 3)]
        [Range(0, 100, ErrorMessage = "Grade must be between 0-100.")]
        public int ExamB { get; set; }
    }
}
