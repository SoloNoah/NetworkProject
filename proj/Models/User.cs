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
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 20")]
        public string Password { get; set; }
        [Required]
        public int PermissionType { get; set; }
    }
}