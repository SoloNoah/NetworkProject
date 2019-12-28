using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using proj.dal;
using proj.Models;
using proj.viewModel;

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
            string username = Request.Form["Username"].ToString();
            string password = Request.Form["Password"].ToString();
            List<User> objUsers = (from x
                                   in userDal.users
                                   where x.Username.Equals(username)
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
        public ActionResult StudentActions()
        {
            if(Request.Form["schedule"] != null)
            {
                CoursesDal coursesDal = new CoursesDal();
                List<Courses> objCourses = (from x
                                            in coursesDal.courses
                                            select x).ToList<Courses>();
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = objCourses;
                return View("showCourses", cvm);
            }
            else
            {
                return View("Login");
            }
            
        }
        public ActionResult showExam()
        {
            CoursesDal coursesDal = new CoursesDal();
            List<Courses> objCourses = coursesDal.courses.ToList<Courses>();
            CoursesViewModel cvm = new CoursesViewModel();
            cvm.courses = objCourses;
            return View("showCourses", cvm);

        }
    }
}