using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj.Models
{
    [Table("tblFaMembers")]
    public class faMember
    {
        [Required]
        [Key, Column(Order = 0)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 20")]
        public string Username { get; set; }
        
        public int Id { get; set; }
        [Required]
        
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 15")]
        public string FirstName { get; set; }
        [Required]
        
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Password must be between 2 and 15")]
        public string LastName { get; set; }
    }
}