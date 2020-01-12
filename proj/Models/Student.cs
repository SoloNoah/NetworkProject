﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proj.Models
{
    [Table("tblStudents")]
    public class Student
    {
        [Required]
        [Key, Column(Order = 0)]
        public string Username { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public int Id { get; set; }
        [Required]
        [Column(Order = 2)]
        public string FirstName { get; set; }
        [Required]
        [Column(Order = 3)]
        public string LastName { get; set; }

    }
}