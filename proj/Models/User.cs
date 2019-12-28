using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace proj.Models
{
    public class User
    {
        [Required]
        [Key]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50")]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 50")]
        public string Password { get; set; }
        [Required]
        public int PermissionType { get; set; }
    }
}