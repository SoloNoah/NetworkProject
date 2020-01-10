using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proj.Models;
namespace proj.viewModel
{
    public class UserViewModel
    {
        public User user { get; set; }
        public List<User> UsersList { get; set; }
    }
}