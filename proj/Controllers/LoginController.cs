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
        public ActionResult login(){
            return View();
        }
        public ActionResult getPermission(){
            UserDal userDal = new UserDal();
            String loggedUsername = Request.Form["Username"].ToString();
            Session["loggedUsername"] = loggedUsername;
            string password = Request.Form["Password"].ToString();
            List<User> objUsers = (from x
                                   in userDal.users
                                   where x.Username.Equals(loggedUsername) 
                                   select x).ToList<User>();
            if (objUsers.Any()){
                User user = objUsers[0];
                if (user.Password.Equals(password)) {
                    switch (user.PermissionType){
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
        public ActionResult StudentActions(String loggedUsername){
            //string loggedUser = TempData["loggedUsername"].ToString();
            string loggedUser = Session["loggedUsername"].ToString() ;
            if (Request.Form["schedule"] != null){
                StudentCoursesDal courseUser = new StudentCoursesDal();
                CoursesDal coursesDal = new CoursesDal();
                string t;
                List<Courses> userCourses = new List<Courses>();

                //extracting username and then all courses that the user have from db.
               
                List<string> c_id = (from y in courseUser.CoursesAndUsers
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();

                int i = 0;
                while (i < c_id.Count()){
                    t = c_id[i];
                    userCourses.Add((from y in coursesDal.courses
                                     where y.CourseId.Equals(t)
                                     select y).SingleOrDefault());
                    i++;
                }


                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = userCourses;
                return View("ShowCourses", cvm);
            }
            if (Request.Form["exams"] != null){
                StudentCoursesDal courseUser = new StudentCoursesDal();
                ExamsDal examsDal = new ExamsDal();
                string t;
                List<Exams> userExams = new List<Exams>();

                //extracting username and then all courses that the user have from db.

                List<string> c_id = (from y in courseUser.CoursesAndUsers
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count()){
                    t = c_id[i];
                    userExams.Add((from y in examsDal.exams
                                     where y.CourseId.Equals(t) && (y.Moed.Contains("a"))
                                     select y).FirstOrDefault());
                    userExams.Add((from y in examsDal.exams
                                   where y.CourseId.Equals(t) && (y.Moed.Contains("b"))
                                   select y).FirstOrDefault());
                    i++;
                }


                CoursesViewModel cvm = new CoursesViewModel();
                cvm.exams = userExams;
                return View("ShowExams", cvm);
            }
            else
                return View("Login");
        }
        public ActionResult LecturerActions(String loggedUsername){
            string loggedUser = Session["loggedUsername"].ToString();
            if (Request.Form["courses"] != null){
                LectDal lectDal = new LectDal();
                CoursesDal coursesDal = new CoursesDal();
                string t;
                List<Courses> lectCourses = new List<Courses>();
                
                List<string> c_id = (from y in lectDal.lectsCourses
                                     where y.LectName.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count()){
                    t = c_id[i];
                    lectCourses.Add((from y in coursesDal.courses
                                     where y.CourseId.Equals(t)
                                     select y).SingleOrDefault());
                    i++;
                }
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = lectCourses;
                return View("ShowCourses", cvm);
            }
            if (Request.Form["studentList"] != null){
                LectDal lectDal = new LectDal();
                LecturerGotStudsDal studentsDal = new LecturerGotStudsDal();
                List<Student> singleStud = new List<Student>();
                string t;
                List<StudentCourses> studentsInCourse = new List<StudentCourses>();
                StudentCoursesDal studentCourseDal = new StudentCoursesDal();
                CoursesDal coursesDal = new CoursesDal();
                List<string> studUsername = new List<string>();

                List<string> c_id = (from y in lectDal.lectsCourses
                                     where y.LectName.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count())
                {
                    t = c_id[i];
                    studUsername = (from y in studentCourseDal.CoursesAndUsers
                                     where y.CourseId.Equals(t)
                                     select y.Username).ToList<string>();
                    i++;
                }

                
                i = 0;
                while(i < studUsername.Count())
                {
                    t = studUsername[i];
                    singleStud.Add((from y in studentsDal.lectsStudents
                                  where y.Username.Contains(t)
                                  select y).FirstOrDefault());
                    i++;
                }
                StudentViewModel svm = new StudentViewModel();
                svm.studentList = singleStud;
                return View("ShowStudsForLects", svm);
            }
            return View();
        }
    }
}