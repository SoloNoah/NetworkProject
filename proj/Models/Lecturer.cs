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
        public int LectId { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string Username { get; set; }
        [Required]
        public string LectName { get; set; }
        [Required]
        public string LectLastName { get; set; }
        [Required]
        [Key, Column(Order = 4)]
        public string CourseId { get; set; }
        [Required]
        public string CourseName { get; set; }
       
    }
}