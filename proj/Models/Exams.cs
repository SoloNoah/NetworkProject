using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj.Models
{
    [Table("tblDeptExams")]
    public class Exams
    {
        [Required]
        [Key, Column(Order = 0)]
        public string Moed { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string CourseId { get; set; }

        [Required]
        [Column(Order = 2)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Course name must be between 2 and 15")]
        public string CourseName { get; set; }

        [Column(Order = 3)]
        public string Day { get; set; }

        [Column(Order = 4)]
        [Required]
        public string Hour { get; set; }

        [Required]
        [Column(Order = 5)]
        public string Room { get; set; }
    }
}