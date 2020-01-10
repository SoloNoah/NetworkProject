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
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20")]
        public string Username { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20")]
        public string Password { get; set; }
        [Required]
        public int PermissionType { get; set; }
    }
}