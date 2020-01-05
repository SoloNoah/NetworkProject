using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proj.Models
{
    [Table("tblLect")]
    public class Lecturer
    {
        [Required]
        [Key, Column(Order = 0)]
        public string LectId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string LectName { get; set; }
        [Required]
        [Column(Order = 2)]
        public string CourseId { get; set; }
        [Required]
        [Column(Order = 3)]
        public string CourseName { get; set; }
       
    }
}