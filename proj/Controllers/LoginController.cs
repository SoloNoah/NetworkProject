using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proj.dal;
using proj.Models;
using proj.viewModel;
//this is Emilias try to commit and push to git
namespace proj.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult login()
        {

            return View();
        }

        public ActionResult getPermission()
        {
            UserDal userDal = new UserDal();
            String loggedUsername = Request.Form["Username"].ToString();
            TempData["loggedUsername"] = loggedUsername;
            string password = Request.Form["Password"].ToString();
            List<User> objUsers = (from x
                                   in userDal.users
                                   where x.Username.Equals(loggedUsername) 
                                   select x).ToList<User>();
            if (objUsers.Any())
            {
                User user = objUsers[0];
                if (user.Password.Equals(password))
                {
                    switch (user.PermissionType)
                    {
                        case 1:
                            return View("Student");
                        case 2:
                            return View("Lecturer");
                        case 3:
                            return View("FaMember");

                    }
                }
            }
            return View("Login");
        }
        public ActionResult StudentActions(String loggedUsername)
        {
            if(Request.Form["schedule"] != null)
            {
                CourseUserDal courseUser = new CourseUserDal();
                CoursesDal coursesDal = new CoursesDal();
               string t;
                List<Courses> userCourses = new List<Courses>();
                string loggedUser = TempData["loggedUsername"].ToString();
                List<string> c_id = (from y in courseUser.CoursesAndUsers
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();

                int i = 0;
                while (i < c_id.Count())
                {
                    t = c_id[i];
                    userCourses.Add((from y in coursesDal.Courses
                                     where y.CourseId.Equals(t)
                                     select y).SingleOrDefault());
                    i++;
                }


                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = userCourses;
                return View("showCourses", cvm);
            }
            else 
            {
                return View("Login");
            }
            
        }
        public ActionResult showExam()
        {
           
            return View("Login");

        }
    }
}