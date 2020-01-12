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
        public string CourseId { get; set; }

       
        [Required]
        public string CourseName { get; set; }

        [Required]
        [Key, Column(Order = 2)]
        public string Moed { get; set; }

        [Key, Column(Order = 3)]
        public DateTime ExamDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan SHour { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan EHour { get; set; }

        [Required]
        public string Room { get; set; }
    }
}