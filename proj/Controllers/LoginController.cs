using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
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
        public ActionResult login() {
            return View();
        }

        public ActionResult getPermission() {
            UserDal userDal = new UserDal();
            String loggedUsername = Request.Form["Username"].ToString();
            Session["loggedUsername"] = loggedUsername;
            string password = Request.Form["Password"].ToString();
            List<User> objUsers = (from x
                                   in userDal.users
                                   where x.Username.Equals(loggedUsername)
                                   select x).ToList<User>();
            if (objUsers.Any()) {
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
                else
                {
                    Session["loginValid"] = "not validated-password";
                    return View("Login");
                }
            }
            Session["loginValid"] = "not validated-username";
            return View("Login");
        }

        public ActionResult StudentActions(String loggedUsername) {
            //string loggedUser = TempData["loggedUsername"].ToString();
            string loggedUser = Session["loggedUsername"].ToString();
            if (Request.Form["schedule"] != null) {
                StudentCoursesDal courseUser = new StudentCoursesDal();
                CoursesDal coursesDal = new CoursesDal();
                string t;
                List<Courses> userCourses = new List<Courses>();
                List<string> c_id = (from y in courseUser.CoursesAndUsers
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count()) {
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
            if (Request.Form["exams"] != null) {
                StudentCoursesDal courseUser = new StudentCoursesDal();
                ExamsDal examsDal = new ExamsDal();
                string t;
                List<Exams> userExams = new List<Exams>();
                List<string> c_id = (from y in courseUser.CoursesAndUsers
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count()) {
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

        public ActionResult LecturerActions(String loggedUsername) {
            string loggedUser = Session["loggedUsername"].ToString();
            if (Request.Form["courses"] != null) {
                LectDal lectDal = new LectDal();
                CoursesDal coursesDal = new CoursesDal();
                string t;
                List<Courses> lectCourses = new List<Courses>();

                List<string> c_id = (from y in lectDal.lectsCourses
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count()) {
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
            if (Request.Form["studentList"] != null) {
                LectDal lectDal = new LectDal();
                LecturerGotStudsDal studentsDal = new LecturerGotStudsDal();
                List<Student> singleStud = new List<Student>();
                string t;
                List<StudentCourses> studentsInCourse = new List<StudentCourses>();
                StudentCoursesDal studentCourseDal = new StudentCoursesDal();
                CoursesDal coursesDal = new CoursesDal();
                List<string> studUsername = new List<string>();
                List<Courses> lectCourses = new List<Courses>();
                List<string> c_id = (from y in lectDal.lectsCourses
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count())
                {
                    t = c_id[i];
                    lectCourses.Add((from y in coursesDal.courses
                                     where y.CourseId.Equals(t)
                                     select y).SingleOrDefault());
                    i++;
                }
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = lectCourses;
                return View("ShowStudsForLect", cvm);
            }
            if (Request.Form["studentGrade"] != null){
                LectDal lectDal = new LectDal();
                CoursesDal coursesDal = new CoursesDal();
                string t;
                List<Courses> lectCourses = new List<Courses>();
                List<string> c_id = (from y in lectDal.lectsCourses
                                     where y.Username.Equals(loggedUser)
                                     select y.CourseId).ToList<string>();
                int i = 0;
                while (i < c_id.Count())
                {
                    t = c_id[i];
                    lectCourses.Add((from y in coursesDal.courses
                                     where y.CourseId.Equals(t)
                                     select y).SingleOrDefault());
                    i++;
                }
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = lectCourses;
                return View("manageExams", cvm);

            }
                return View();
        }

        public ActionResult enter(String loggedUsername, string courseId)
        {
            string loggedUser = Session["loggedUsername"].ToString();
            String cousreid = courseId;
            LectDal lectDal = new LectDal();
            LecturerGotStudsDal studentsDal = new LecturerGotStudsDal();
            List<Student> singleStud = new List<Student>();
            string t;
            List<StudentCourses> studentsInCourse = new List<StudentCourses>();
            StudentCoursesDal studentCourseDal = new StudentCoursesDal();
            CoursesDal coursesDal = new CoursesDal();
            List<string> studUsername = new List<string>();
            List<Courses> lectCourses = new List<Courses>();
            List<string> c_id = (from y in lectDal.lectsCourses
                                 where y.Username.Equals(loggedUser)
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
            while (i < studUsername.Count())
            {
                t = studUsername[i];
                singleStud.Add((from y in studentsDal.lectsStudents
                                where y.Username.Contains(t)
                                select y).FirstOrDefault());
                i++;
            }
            StudentViewModel svm = new StudentViewModel();
            svm.studentList = singleStud;
            return View("enter", svm);
        }

        public ActionResult FaMemberActions() {
            if(Request.Form["ManageCoursesAndStudents"] != null){
                CoursesDal coursesDal = new CoursesDal();
                StudentDal studentsDal = new StudentDal();
                List<Student> students = (from x
                                          in studentsDal.allStudents
                                          select x).ToList<Student>();
                List<Courses> courses = (from x
                                        in coursesDal.courses
                                         select x).ToList<Courses>();
                faMemberViewModel fvm = new faMemberViewModel();
                fvm.faStudents = students;
                fvm.faCourses = courses;
                return View("manageStudents", fvm);
            }
            if(Request.Form["ManageCoursesSchedule"] != null){
                CoursesDal coursesDal = new CoursesDal();
                List<Courses> courses = (from x
                                       in coursesDal.courses
                                         select x).ToList<Courses>();
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = courses;
                return View("manageCourses", cvm);
            }
            if(Request.Form["ManageExams"] != null){
                ExamsDal examdal = new ExamsDal();
                CoursesDal coursesDal = new CoursesDal();
                List<Exams> exams = (from x
                                     in examdal.exams
                                     select x).ToList<Exams>();
                List<Courses> courses = (from x
                                      in coursesDal.courses
                                         select x).ToList<Courses>();
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.exams = exams;
                cvm.courses = courses;
                return View("manageExams", cvm);
            }
            return View();
        }

        public ActionResult assignStudents(int studentId, string courseName) {
            int sid = studentId;
            string cname = courseName;
            CoursesDal coursedal = new CoursesDal();
            StudentDal stud = new StudentDal();
            StudentCoursesDal studentcoursedal = new StudentCoursesDal();
            //pulling the choosen course info
            Courses cid = (from x
                           in coursedal.courses
                           where x.CourseName.Equals(cname)
                           select x).FirstOrDefault<Courses>();
            //pulling the choosen student info
            Student sname = (from x
                            in stud.allStudents
                             where x.Id.Equals(sid)
                             select x).FirstOrDefault<Student>();
            //checking if the student is already assigned to that course
            bool rowExists = FindRow(sname.Username, cid.CourseId,"student");
            //if row true = means row does not exists in the table
            if (rowExists)
            {
                //all the courses of the student
                List<string> studentCoursesID = new List<string>();
                List<Courses> courses = new List<Courses>();
                studentCoursesID = (from x
                                  in studentcoursedal.CoursesAndUsers
                                    where x.Username.Equals(sname.Username)
                                    select x.CourseId).ToList<string>();
                int i = 0;
                while (i < studentCoursesID.Count())
                {
                    string t = studentCoursesID[i];
                    courses.Add((from y in coursedal.courses
                                 where y.CourseId.Equals(t)
                                 select y).SingleOrDefault());
                    i++;
                }
                //checking if the new course day&hour clashes with the existing student`s courses.
                bool result = isClashing(cid, courses);
                //if result true = means the day&hour does clash with the existing student`s courses.
                if (result)
                {
                    Session["result"] = "Operation failed for student";
                }
                else
                {
                    StudentCourses studentcourses = new StudentCourses { CourseId = cid.CourseId, Username = sname.Username, ExamA = 0, ExamB = 0 };
                    studentcoursedal.CoursesAndUsers.Add(studentcourses);
                    studentcoursedal.SaveChanges();
                    Session["rowExists"] = "success";
                }
            }
            else
                Session["rowExists"] = "student already assinged";
        return View("FaMember");
    }

        public bool FindRow(string sname, string cid, string msg)
        {
            if (msg == "student")
            {
                StudentCoursesDal scd = new StudentCoursesDal();
                if (scd.CoursesAndUsers.Any(o => o.CourseId.Equals(cid) && o.Username.Equals(sname)))
                    return false;
                return true;
            }
            if(msg == "lecturer")
            {
                LectDal ld = new LectDal();
                if (ld.lectsCourses.Any(o => o.CourseId.Equals(cid) && o.Username.Equals(sname)))
                    return false;
                return true;
            }
            return false;
        }

        public bool isClashing(Courses givencourse, List<Courses> coursesToCheck)
        {
            int i = 0;
            while (i < coursesToCheck.Count())
            {
                if (coursesToCheck[i].Day == givencourse.Day)
                {
                    bool a = givencourse.SHour <= coursesToCheck[i].SHour && givencourse.EHour <= coursesToCheck[i].EHour && 
                        givencourse.EHour > coursesToCheck[i].SHour;
                    bool b = givencourse.SHour > coursesToCheck[i].SHour && givencourse.EHour < coursesToCheck[i].EHour;
                    bool c = givencourse.SHour < coursesToCheck[i].SHour && givencourse.EHour > coursesToCheck[i].EHour;
                    bool d = givencourse.SHour > coursesToCheck[i].SHour && givencourse.EHour > coursesToCheck[i].EHour &&
                        givencourse.SHour < coursesToCheck[i].EHour;
                    if (a || b || c || d)
                        return true;
                    return false;
                }
                i++;
            }
            return false;
        }

        public bool isClashingExams(Exams givenExam, List<Exams> examsToCheck)
        {
            int i = 0;
            while (i < examsToCheck.Count())
            {
                if (examsToCheck[i].Day == givenExam.Day)
                {
                    if (examsToCheck[i].Moed == givenExam.Moed)
                    {
                        bool a = givenExam.SHour <= examsToCheck[i].SHour && givenExam.EHour <= examsToCheck[i].EHour &&
                            givenExam.EHour > examsToCheck[i].SHour;
                        bool b = givenExam.SHour > examsToCheck[i].SHour && givenExam.EHour < examsToCheck[i].EHour;
                        bool c = givenExam.SHour < examsToCheck[i].SHour && givenExam.EHour > examsToCheck[i].EHour;
                        bool d = givenExam.SHour > examsToCheck[i].SHour && givenExam.EHour > examsToCheck[i].EHour &&
                            givenExam.SHour < examsToCheck[i].EHour;
                        if (a || b || c || d)
                            return true;
                        return false;
                    }
                }
                i++;
            }
            return false;
        }

        public ActionResult assignLecturers(int LectId, string courseName)
        {
            int Lid = LectId;
            string cname = courseName;
            CoursesDal coursedal = new CoursesDal();
            LectDal lectdal = new LectDal();
            
            //pulling the choosen course info
            Courses cid = (from x
                           in coursedal.courses
                           where x.CourseName.Equals(cname)
                           select x).FirstOrDefault<Courses>();
            //pulling the choosen lect info
            Lecturer Lname = (from x
                            in lectdal.lectsCourses
                             where x.LectId.Equals(Lid)
                             select x).FirstOrDefault<Lecturer>();
            //checking if the student is already assigned to that course
            bool rowExists = FindRow(Lname.Username, cid.CourseId,"lecturer");
            //if row true = means row does not exists in the table
            if (rowExists)
            {
                //all the courses of the lect
                List<string> LectCoursesID = new List<string>();
                List<Courses> courses = new List<Courses>();
                LectCoursesID = (from x
                                  in lectdal.lectsCourses
                                  where x.LectId.Equals(Lid)
                                  select x.CourseId).ToList<string>();
                int i = 0;
                while (i < LectCoursesID.Count())
                {
                    string t = LectCoursesID[i];
                    courses.Add((from y in coursedal.courses
                                 where y.CourseId.Equals(t)
                                 select y).SingleOrDefault<Courses>());
                    i++;
                }
                //checking if the new course day&hour clashes with the existing lecturer`s courses.
                bool result = isClashing(cid, courses);
                //if result true = means the day&hour does clash with the existing lecturer`s courses.
                if (result)
                {
                    Session["result"] = "Operation failed";
                }
                else
                {
                    Lecturer lecturer = new Lecturer { LectId=Lid, Username = Lname.Username, LectName=Lname.LectName, LectLastName=Lname.LectLastName, CourseId=cid.CourseId , CourseName =cid.CourseName };
                    lectdal.lectsCourses.Add(lecturer);
                    lectdal.SaveChanges();
                    Session["rowExists"] = "Lecturer assinged";
                }
            }
            else
                Session["rowExists"] = "Lecturer already assinged";
            return View("FaMember");
        }

        public ActionResult FaMupdateGrade(string courseID){
            string cid = courseID;
            StudentCoursesDal studentCoursesDal = new StudentCoursesDal();
            //usernames from the joined tbl
            List<string> studentsIDS = (from x
                                        in studentCoursesDal.CoursesAndUsers
                                        where x.CourseId.Equals(cid)
                                        select x.Username).ToList<string>();
            //this shitty list contains the grades of the students. not sure why we made it that way.
            //Noah if u read dis know that i had zero powers to do it nicely.
            List<StudentCourses> studentcourses = (from x
                                                   in studentCoursesDal.CoursesAndUsers
                                                   where x.CourseId.Equals(cid)
                                                   select x).ToList<StudentCourses>();
            //list of student from the students list according to their IDs.
            StudentDal studentdal = new StudentDal();
            List<Student> students = new List<Student>();
            int i = 0;
            while (i < studentsIDS.Count())
            {
               string current = studentsIDS[i];
               students.Add((from x
                             in studentdal.allStudents
                             where x.Id.Equals(current)
                             select x).FirstOrDefault());
                i++;
            }
            LecturerViewModel sdv = new LecturerViewModel();
            sdv.students = students;
            //sdv.studentcourses = studentcourses;
            return View("login",sdv);
         }

        public List<int> distinct(List<Lecturer> lects,LectDal lectdal)
        {
            List<int> distincedLects = new List<int>();
            int i = 0;
            while (i < lects.Count()){
                distincedLects = (from x
                                   in lectdal.lectsCourses
                                   select x.LectId).ToList<int>();
                i++;
            }
            List<int> disLec = distincedLects.Distinct().ToList<int>();
            return disLec;
        }

        public ActionResult addCourse()
        {
            //extracting the info from faMember inputs.
            string courseId = Request.Form["CourseId"].ToString();
            string courseName = Request.Form["CourseName"].ToString();
            string Day = Request.Form["Day"].ToString();
            string Room = Request.Form["Room"].ToString();
            string StartHour = Request.Form["SHour"].ToString();
            TimeSpan SHour = TimeSpan.Parse(StartHour);
            string EndHour = Request.Form["EHour"].ToString();
            TimeSpan EHour = TimeSpan.Parse(EndHour);
            Courses newC = new Courses
            {
                CourseId = courseId,
                CourseName = courseName,
                Day = Day,
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            CoursesDal coursesDal = new CoursesDal();
            List<Courses> courses = (from x
                                    in coursesDal.courses
                                        select x).ToList<Courses>();
            //checking if the info above clashes with exsisting courses.
            bool clashes = isClashing(newC, courses);
            //if clashes true = means the day&hour does clash with the existing courses.
            if (clashes)
            {
                Session["courseSaved"] = "course clashes with other courses!";
            }
            else
            {
                coursesDal.courses.Add(newC);
                coursesDal.SaveChanges();
                Session["courseSaved"] = "the course has been added!";
            }
        return View("FaMember");
        }

        public ActionResult deleteCourse(string courseId)
        {
            CoursesDal cdal = new CoursesDal();
            StudentCoursesDal scdal = new StudentCoursesDal();
            //extracting the relevant course to be deleted.
            Courses courseTBD = (cdal.courses.Single(c => c.CourseId.Equals(courseId)));
            StudentCourses studentCourseTBD = (scdal.CoursesAndUsers.Single(c => c.CourseId.Equals(courseId)));
            cdal.courses.Remove(courseTBD);
            scdal.CoursesAndUsers.Remove(studentCourseTBD);
            cdal.SaveChanges();
            scdal.SaveChanges();
            Session["coursedeleted"] = "the course has been deleted!";
            return View("FaMember");

        }

        public ActionResult editCourses(string courseId)
        {
            //extracting the info from faMember inputs.
            string courseid = courseId;
            string Day = Request.Form["Day"].ToString();
            string Room = Request.Form["Room"].ToString();
            string StartHour = Request.Form["SHour"].ToString();
            TimeSpan SHour = TimeSpan.Parse(StartHour);
            string EndHour = Request.Form["EHour"].ToString();
            TimeSpan EHour = TimeSpan.Parse(EndHour);
            CoursesDal cdal = new CoursesDal();
            //saving the old course
            Courses c = (cdal.courses.Single(x => x.CourseId.Equals(courseId)));
            string cname = c.CourseName;
            //crating new course with new dits.
            Courses newC = new Courses
            {
                CourseId = courseid,
                CourseName = cname,
                Day = Day,
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            List<Courses> courses = (from x
                                    in cdal.courses
                                    select x).ToList<Courses>();
            //checking if the info above clashes with exsisting courses.
            bool clashes = isClashing(newC, courses);
            if (clashes)
            {
                Session["courseSaved"] = "course clashes with other courses!";
            }
            else
            {
                cdal.courses.Remove(c);
                cdal.courses.Add(newC);
                cdal.SaveChanges();
                Session["courseSaved"] = "the course has been added!";
            }
            return View("FaMember");

        }

        public ActionResult addExam()
        {
            //extracting the info from faMember inputs.
            string moed = Request.Form["Moed"].ToString();
            string courseId = Request.Form["CourseId"].ToString();
            string courseName = Request.Form["CourseName"].ToString();
            string Day = Request.Form["Day"].ToString();
            string Room = Request.Form["Room"].ToString();
            string StartHour = Request.Form["SHour"].ToString();
            TimeSpan SHour = TimeSpan.Parse(StartHour);
            string EndHour = Request.Form["EHour"].ToString();
            TimeSpan EHour = TimeSpan.Parse(EndHour);
            Exams newE = new Exams()
            {
                Moed = moed,
                CourseId = courseId,
                CourseName = courseName,
                Day = Day,
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            ExamsDal examsDal = new ExamsDal();
            List<Exams> exams = (from x
                                 in examsDal.exams
                                 select x).ToList<Exams>();
            //checking if the info above clashes with exsisting courses.
            bool clashes = isClashingExams(newE, exams);
            //if clashes true = means the day&hour does clash with the existing courses.
            if (clashes)
            {
                Session["examSaved"] = "exam clashes with other exams!";
            }
            else
            {
                examsDal.exams.Add(newE);
                examsDal.SaveChanges();
                Session["courseSaved"] = "the exam has been added!";
            }
            return View("FaMember");
        }

        public ActionResult deleteExam(string courseId,string moed)
        {
            ExamsDal edal = new ExamsDal();
            //extracting the relevant course to be deleted.
            Exams examTBD = (edal.exams.Single(c => c.CourseId.Equals(courseId) && c.Moed.Equals(moed)));
            edal.exams.Remove(examTBD);
            edal.SaveChanges();
            Session["examdeleted"] = "the exam has been deleted!";
            return View("FaMember");
        }

        public ActionResult editExam(string courseId, string moed)
        {
            //extracting the info from faMember inputs.
            string courseid = courseId;
            string eMoed = moed;
            string Day = Request.Form["Day"].ToString();
            string Room = Request.Form["Room"].ToString();
            string StartHour = Request.Form["SHour"].ToString();
            TimeSpan SHour = TimeSpan.Parse(StartHour);
            string EndHour = Request.Form["EHour"].ToString();
            TimeSpan EHour = TimeSpan.Parse(EndHour);
            ExamsDal edal = new ExamsDal();
            //saving the old course
            Exams e = (edal.exams.Single(c => c.CourseId.Equals(courseId) && c.Moed.Equals(moed)));
            string cname = e.CourseName;
            //crating new course with new dits.
            Exams newE = new Exams
            {
                Moed = eMoed,
                CourseId = courseid,
                CourseName = cname,
                Day = Day,
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            List<Exams> exams = (from x
                                   in edal.exams
                                   select x).ToList<Exams>();
            //checking if the info above clashes with exsisting courses.
            bool clashes = isClashingExams(newE, exams);
            if (clashes)
            {
                Session["examSaved"] = "exam clashes with other courses!";
            }
            else
            {
                edal.exams.Remove(e);
                edal.exams.Add(newE);
                edal.SaveChanges();
                Session["examSaved"] = "the exam has been added!";
            }
            return View("FaMember");

        }
    }
}
