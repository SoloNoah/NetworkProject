using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace proj.Models
{
    [Table("tblUsers")]
    public class User
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20")]
        public string Password { get; set; }
        [Required]
        public int PermissionType { get; set; }
    }
}