using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            return View(new User());
        }
        public ActionResult GetPermission(User input) {
            //sending input to CheckLogin that return true if input is correct compared to db. false if user+password isn't found.
            //if found, the program checks the permission type and directs to the proper view.
            if (ModelState.IsValid)
            {
                if (CheckLogin(Request.Form["Username"].ToString(), Request.Form["Password"].ToString()))
                {
                    List<User> user = GetUserRecord(Request.Form["Username"].ToString());
                    Session["loggedUsername"] = user[0].Username;
                    switch (user[0].PermissionType)
                    {
                        case 1:
                            {
                                StoreUserData(Request.Form["Username"].ToString(), user[0].PermissionType);
                                return View("Student", Session["firstname"]);
                            }
                        case 2:
                            {
                                StoreUserData(Request.Form["Username"].ToString(), user[0].PermissionType);
                                return View("Lecturer", Session["firstname"]);
                            }
                        case 3:
                            {
                                StoreUserData(Request.Form["Username"].ToString(), user[0].PermissionType);
                                return View("FaMember", Session["firstname"]);
                            }
                    }
                }
                ViewBag.PromptMessage = "Invalid Credentials Supplied";
            }
            return View("Login", input);
        }
        public ActionResult StudentActions(String loggedUsername) {

            //if user, a student, wants to see the schedule this method saves all the courses id in a list of string
            // and then finds all courses from db with those course ids. sends them to view and shows results in a table.
            if (Request.Form["schedule"] != null) {
                StudentCoursesDal courseUser = new StudentCoursesDal();
                List<Courses> userCourses = new List<Courses>();
                CoursesDal coursesDal = new CoursesDal();

                //extracting username and then all courses that the user have from db.

                List<string> c_id = GetStudentCourseId(Session["loggedUsername"].ToString());
                CoursesViewModel cvm = new CoursesViewModel();
                userCourses = GetCourseInfoPerCourseId(c_id);
                cvm.courses = userCourses;
                return View("ShowCourses", cvm);
            }
            if (Request.Form["exams"] != null) {
                StudentCoursesDal courseUser = new StudentCoursesDal();
               ` ExamsDal examsDal = new ExamsDal();
                string t;
                List<Exams> userExams = new List<Exams>();

                //extracting username and then all exams that the user have from db.

                List<string> c_id = GetStudentCourseId(Session["loggedUsername"].ToString());
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
            if (Request.Form["courses"] != null)
            {
                //getting all courses that the lecturer teaches.
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = GetAllCourses(loggedUser);
                return View("ShowLectCourses", cvm);
            }
           
            return View();
        }
        public ActionResult FaMemberActions()
        {
            if (Request.Form["ManageCoursesAndStudents"] != null)
            {
                CoursesDal coursesDal = new CoursesDal();
                StudentDal studentsDal = new StudentDal();
                List<Student> students = (from x
                                          in studentsDal.allStudents
                                          select x).ToList<Student>();
                List<Courses> courses = (from x
                                        in coursesDal.courses
                                         select x).ToList<Courses>();
                FacultyMemberViewModel fvm = new FacultyMemberViewModel();
                fvm.faStudents = students;
                fvm.faCourses = courses;
                return View("ManageStudents", fvm);
            }
            if (Request.Form["ManageCoursesAndLects"] != null)
            {
                LectDal lectdal = new LectDal();
                CoursesDal coursesdal = new CoursesDal();
                List<Lecturer> lecturers = (from x
                                            in lectdal.lectsCourses
                                            select x).ToList<Lecturer>();
                List<Courses> courses = (from x
                                         in coursesdal.courses
                                         select x).ToList<Courses>();

                List<int> lecturersID = DistinctWithLects(lecturers, lectdal);
                FacultyMemberViewModel fvm = new FacultyMemberViewModel();
                fvm.faLecturers = lecturers;
                fvm.faCourses = courses;
                fvm.faLecturersID = lecturersID;
                return View("ManageLecturer", fvm);
            }
            if (Request.Form["ManageCourseSchedule"] != null)
            {
                CoursesDal coursesDal = new CoursesDal();
                List<Courses> courses = (from x
                                       in coursesDal.courses
                                         select x).ToList<Courses>();
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.courses = courses;
                return View("ManageCourses", cvm);
            }
            if (Request.Form["ManageExams"] != null)
            {
                ExamsDal examdal = new ExamsDal();
                CoursesDal coursesDal = new CoursesDal();
                List<Exams> exams = (from x
                                     in examdal.exams
                                     select x).ToList<Exams>();
                List<Courses> courses = (from x
                                          in coursesDal.courses
                                         select x).ToList<Courses>();
                CoursesViewModel cvm = new CoursesViewModel();
                cvm.examsId = DistinctWithExams(exams, examdal);
                cvm.exams = exams;
                cvm.courses = courses;
                return View("ManageExams", cvm);
            }
            if (Request.Form["UpdateGrades"] != null)
            {
                List<StudentCourses> allStudents = new List<StudentCourses>();
                StudentCoursesDal studentCoursesDal = new StudentCoursesDal();
                CoursesUserViewModel cvm = new CoursesUserViewModel();
                allStudents = (from x in studentCoursesDal.CoursesAndUsers
                               select x).ToList<StudentCourses>();
                cvm.allStudentsCourses = allStudents;
                return View("UpdateGradesFaculty", cvm);
            }
            return View("login");
        }
        public ActionResult ShowStudentForCourse(string courseId)
        {
            LecturerGotStudsDal studentsDal = new LecturerGotStudsDal();
            StudentCoursesDal studentCourseDal = new StudentCoursesDal();
            List<string> studUsername = new List<string>();
            List<StudentCourses> studsList = new List<StudentCourses>();

            CoursesUserViewModel svm = new CoursesUserViewModel();

            studsList = (from y in studentCourseDal.CoursesAndUsers
                         where y.CourseId.Equals(courseId)
                         select y).ToList<StudentCourses>();

            svm.allStudentsCourses = studsList;
            ViewData["course"] = GetCourseName(courseId); 
            return View("ShowStudentsForCourse", svm);
        }
        public ActionResult AssignStudents(int studentId, string courseName)
        {
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
            bool rowExists = CheckIfRowExists(sname.Username, cid.CourseId, "student");
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
                bool result = CheckCourseTimeClash(cid, courses);
                //if result true = means the day&hour does clash with the existing student`s courses.
                if (result)
                {
                    Session["result"] = "Operation failed";
                }
                else
                {
                    StudentCourses studentcourses = new StudentCourses { CourseId = cid.CourseId, Username = sname.Username, ExamA = 0, ExamB = 0 };
                    studentcoursedal.CoursesAndUsers.Add(studentcourses);
                    studentcoursedal.SaveChanges();
                    Session["result"] = "success";
                }
            }
            else
                Session["rowExists"] = "student already assinged";
            return View("FaMember");
        }

        public Courses GetCourseInfo(string courseName)
        {
            CoursesDal coursedal = new CoursesDal();

            return (from x in coursedal.courses
                    where x.CourseName.Equals(courseName)
                    select x).FirstOrDefault<Courses>();

        }
        public Lecturer GetLecturerInfo(int lectId)
        {
            LectDal lectdal = new LectDal();
            return (from x
                    in lectdal.lectsCourses
                    where x.LectId.Equals(lectId)
                    select x).FirstOrDefault<Lecturer>();
        }
        public List<string> GetCourseId(int lectId)
        {
            LectDal lectdal = new LectDal();
            return (from x  
                    in lectdal.lectsCourses
                    where x.LectId.Equals(lectId)
                    select x.CourseId).ToList<string>();
        }
        public ActionResult AssignLecturers(int lectId, string courseName)
        {
            CoursesDal coursedal = new CoursesDal();
            LectDal lectDal = new LectDal();
            //pulling the choosen course info
            Courses cid = GetCourseInfo(courseName);
            //pulling the choosen lect info
            Lecturer lecturer = GetLecturerInfo(lectId);
            //checking if the lecturer is already assigned to that course          
            //if row true = means row does not exists in the table
            if (CheckIfRowExists(lecturer.Username, cid.CourseId, "lecturer"))
            {
                //all the courses of the lect
                List<string> lectCoursesID = new List<string>();
                List<Courses> courses = new List<Courses>();
                lectCoursesID = GetCourseId(lectId);
                courses = GetCourseInfoPerCourseId(lectCoursesID);     
                //checking if the new course day&hour clashes with the existing lecturer`s courses.
                //if result true = means the day&hour does clash with the existing lecturer`s courses.
                if (CheckCourseTimeClash(cid, courses))
                {
                    Session["result"] = "Operation failed";
                }
                else
                {
                    Lecturer newLecturer = new Lecturer { LectId = lectId, Username = lecturer.Username, LectName = lecturer.LectName, LectLastName = lecturer.LectLastName, CourseId = cid.CourseId, CourseName = cid.CourseName };
                    lectDal.lectsCourses.Add(newLecturer);
                    lectDal.SaveChanges();
                    Session["rowExists"] = "Lecturer assinged";
                }
            }
            else
                Session["rowExists"] = "Lecturer already assinged";
            return View("FaMember");
        }
        public List<Courses> GetAllCourses(string loggedUser)
        {
            //getting logged user and getting the course ids in c_id.
            //returning the relevant list of courses.
            List<string> c_id;
            c_id = GetLectCourseQuery(loggedUser);

            return GetCourseInfoPerCourseId(c_id);
        }
        public List<Courses> GetCourseInfoPerCourseId(List<string> c_id)
        {
            //getting list of course ids and  creating list of List<courses> with info that matches the course's id in c_id.
            List<Courses> courses = new List<Courses>();
            CoursesDal coursesDal = new CoursesDal();
            string holder;
            int i = 0;
            while (i < c_id.Count())
            {
                holder = c_id[i];
                courses.Add((from y in coursesDal.courses
                                 where y.CourseId.Equals(holder)
                                 select y).SingleOrDefault());
                i++;
            }
            return courses;
        }
        public List<Courses> GetLectExams(string loggedUser)
        {
            string holder;
            int i = 0;
            CoursesDal coursesDal = new CoursesDal();
            List<string> c_id;
            List<Courses> lectCourses = new List<Courses>();
            CoursesViewModel cvm = new CoursesViewModel();

            c_id = GetLectCourseQuery(loggedUser);
            while (i < c_id.Count())
            {
                holder = c_id[i];
                lectCourses.Add((from y in coursesDal.courses
                                 where y.CourseId.Equals(holder)
                                 select y).SingleOrDefault());
                i++;
            }
            return lectCourses;
        }
        public ActionResult AddCourse()
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
            bool clashes = CheckCourseTimeClash(newC, courses);
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
        public ActionResult DeleteCourse(string courseId)
        {
            CoursesDal cdal = new CoursesDal();
            LectDal ldal = new LectDal();
            StudentCoursesDal scdal = new StudentCoursesDal();
            //extracting the relevant course to be deleted.
            Courses courseTBD = (cdal.courses.Single(c => c.CourseId.Equals(courseId)));
            List<StudentCourses> studentCourseTBD = (from x 
                                                     in scdal.CoursesAndUsers
                                                     where x.CourseId.Equals(courseId)
                                                     select x).ToList<StudentCourses>();
            List<Lecturer> lecturerTbd = (from x
                                          in ldal.lectsCourses
                                          where x.CourseId.Equals(courseId)
                                          select x).ToList<Lecturer>();
            cdal.courses.Remove(courseTBD);
            for(int i=0; i < studentCourseTBD.Count(); i++)
            {
                scdal.CoursesAndUsers.Remove(studentCourseTBD[i]);

            }
            for (int i = 0; i < lecturerTbd.Count(); i++)
            {
               ldal.lectsCourses.Remove(lecturerTbd[i]);

            }
            try {
                cdal.SaveChanges();
                scdal.SaveChanges();
                ldal.SaveChanges();
            } catch (Exception) { };
           
            Session["coursedeleted"] = "the course has been deleted!";
            return View("FaMember");

        }
        public ActionResult EditCourses(string courseId)
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
            bool clashes = CheckCourseTimeClash(newC, courses);
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
        public ActionResult AddExam()
        {
            //extracting the info from faMember inputs.
            string moed = Request.Form["Moed"].ToString();
            string courseId = Request.Form["CourseId"].ToString();
            string courseName = Request.Form["CourseName"].ToString();
            string date = Request.Form["Day"].ToString();
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
                ExamDate = Convert.ToDateTime(date),
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            ExamsDal examsDal = new ExamsDal();
            List<Exams> exams = (from x
                                 in examsDal.exams
                                 select x).ToList<Exams>();
            //checking if the info above clashes with exsisting courses.
            //if clashes true = means the day&hour does clash with the existing courses.
            if (CheckExamTimeClash(newE, exams) && CheckAllMoeds(newE,exams) && CheckMoedClash(newE, exams))
            {
                Session["examSaved"] = "exam clashes with other exams!";
            }
            else
            {
                examsDal.exams.Add(newE);
                try
                {
                    examsDal.SaveChanges();
                }
                catch (Exception) { }; 
                Session["courseSaved"] = "the exam has been added!";
            }
            return View("FaMember");
        }
        public ActionResult DeleteExam(string courseId)
        {
            ExamsDal edal = new ExamsDal();
            DateTime examDate = Convert.ToDateTime(Request.Form["date"]);
            string moed = Request.Form["Moed"].ToString();
            //extracting the relevant course to be deleted.
            Exams examTBD = (edal.exams.Single(c => c.CourseId.Equals(courseId) && c.Moed.Equals(moed) && c.ExamDate.Equals(examDate)));
            edal.exams.Remove(examTBD);
            edal.SaveChanges();
            Session["examdeleted"] = "the exam has been deleted!";
            return View("FaMember");
        }
        public ActionResult EditExam(string courseId)
        {
            //extracting the info from faMember inputs.
            string moed = Request.Form["Moed"].ToString();
            string courseid = courseId;
            string date = Request.Form["Day"].ToString();
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
                Moed = moed,
                CourseId = courseid,
                CourseName = cname,
                ExamDate = Convert.ToDateTime(date),
                SHour = SHour,
                EHour = EHour,
                Room = Room
            };
            List<Exams> exams = (from x
                                   in edal.exams
                                 select x).ToList<Exams>();
            //checking if the info above clashes with exsisting courses.
            if (CheckExamTimeClash(newE, exams) && CheckAllMoeds(newE, exams) && CheckMoedClash(newE, exams))
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

      

        public List<int> DistinctWithLects(List<Lecturer> lects, LectDal lectdal)
            {
                List<int> distincedLects = new List<int>();
                int i = 0;
                while (i < lects.Count())
                {
                    distincedLects = (from x
                                       in lectdal.lectsCourses
                                      select x.LectId).ToList<int>();
                    i++;
                }
                List<int> disLec = distincedLects.Distinct().ToList<int>();
                return disLec;
            }
        public List<string> DistinctWithExams(List<Exams> lects, ExamsDal examDal)
        {
            List<string> distincedExams = new List<string>();
            int i = 0;
            while (i < lects.Count())
            {
                distincedExams = (from x
                                   in examDal.exams
                                  select x.CourseId).ToList<string>();
                i++;
            }
            List<string> disLec = distincedExams.Distinct().ToList<string>();
            return disLec;
        }
        public List<string> GetLectCourseQuery(string loggedUser)
        {
            //getting lect username and sending back the ids of the courses the lecturer teaches.
            LectDal lectDal = new LectDal();

            return (from y in lectDal.lectsCourses
                    where y.Username.Equals(loggedUser)
                    select y.CourseId).ToList<string>();
        }
        public List<string> GetStudentCourseId(string loggedUser)
        {
            //returning values of Student-Course where the username matches the loggeduser.
            StudentCoursesDal courseUser = new StudentCoursesDal();
            return (from y in courseUser.CoursesAndUsers
                    where y.Username.Equals(loggedUser)
                    select y.CourseId).ToList<string>();
        }

        public List<StudentCourses> GetCourseUserQuery(string loggedUser)
        {
            List<string> c_id;
            c_id = GetLectCourseQuery(loggedUser);
            LectDal lectDal = new LectDal();
            List<StudentCourses> smt = new List<StudentCourses>();
            return smt;
        }
        public List<User> GetUserRecord(string loggedUsername)
        {//gets loggedUsername and returns the relevant record from the db.
            UserDal userDal = new UserDal();
            return (from x
                    in userDal.users
                    where x.Username.Equals(loggedUsername)
                    select x).ToList<User>();
        }

        public ActionResult UpdateGrade(string moed)
        {
            //CheckValues checks if the input from user is correct. if false redirected to "fails" view
            //if CheckValue() is true, CheckInputGrade.
            //if CheckInputGrade() isn't -1 it tries updating the value in db by the moed, a or b.
            String studentName = Request.Form["Username"].ToString();
            String courseId = Request.Form["CourseId"].ToString();
            if (CheckValues(courseId, studentName))
            {
                int grade = CheckInputGrade(Request.Form["Grade"].ToString());

                if (grade == -1) { 
                    Session["grade"] = "invalid input grade";
                    return View("Lecturer");
                    }
                else
                {
                    StudentCoursesDal studentCoursesDal = new StudentCoursesDal();
                    StudentCourses studentCourse = new StudentCourses();

                    var oldValue = studentCoursesDal.CoursesAndUsers.FirstOrDefault(s => (s.Username == studentName) && (s.CourseId == courseId));
                    if (oldValue != null)
                    {
                        if (moed.Equals("a"))
                            oldValue.ExamA = grade;
                        else
                            oldValue.ExamB = grade;
                        Session["grade"] = "Updated grade";
                        studentCoursesDal.SaveChanges();
                    }
                }
                return View("Lecturer");
            }
            else
            {
                Session["grade"] = "invalid input grade";
                return View("Lecturer");
            }

        }
        public void StoreUserData(string loggedUsername, int permissionType)
        {
            //stores the firstname of the user after pulling data from the db. 
            //if the user is either lecturer or faculty member the perission type is saved in Session as well.
            if (permissionType == 1)
            {
                LecturerGotStudsDal studDal = new LecturerGotStudsDal();
                List<Student> student = new List<Student>();
                student = (from x
                        in studDal.lectsStudents
                           where x.Username.Equals(loggedUsername)
                           select x).ToList();
                Session["firstname"] = student[0].FirstName;
                return;
            }
            if (permissionType == 2)
            {
                LectDal lectDal = new LectDal();
                List<Lecturer> lecturer = new List<Lecturer>();
                lecturer = (from x
                            in lectDal.lectsCourses
                            where x.Username.Equals(loggedUsername)
                            select x).ToList<Lecturer>();
                Session["firstname"] = lecturer[0].LectName;
                Session["permission"] = "2";
                return;
            }
            if (permissionType == 3)
            {
                FacultyMemeberDal facDal = new FacultyMemeberDal();
                List<FacultyMember> faculty = new List<FacultyMember>();
                faculty = (from x in facDal.facultyData
                           where x.Username.Equals(loggedUsername)
                           select x).ToList<FacultyMember>();
                Session["firstname"] = faculty[0].FirstName;
                Session["permission"] = "3";

                return;
            }

        }

        public string GetCourseName(string cid)
        {
            //return coursename after getting the id.
            CoursesDal coursesDal = new CoursesDal();
            List<Courses> singleCourse = ((from x in coursesDal.courses
                                           where x.CourseId.Equals(cid)
                                           select x)).ToList<Courses>();
            return singleCourse[0].CourseName;
        }

        public bool CheckIfRowExists(string sname, string cid, string msg)
        {
            if (msg == "student")
            {
                StudentCoursesDal scd = new StudentCoursesDal();
                if (scd.CoursesAndUsers.Any(o => o.CourseId.Equals(cid) && o.Username.Equals(sname)))
                    return false;
                return true;
            }
            if (msg == "lecturer")
            {
                LectDal ld = new LectDal();
                if (ld.lectsCourses.Any(o => o.CourseId.Equals(cid) && o.Username.Equals(sname)))
                    return false;
                return true;
            }
            return false;
        }
        public bool CheckCourseTimeClash(Courses givencourse, List<Courses> coursesToCheck)
        {
            for(int i = 0;  i < coursesToCheck.Count(); i++)
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
            }
            return false;
        }

        public bool CheckMoedClash(Exams givenExam, List<Exams> examsToCheck)
        {
           
            for(int i = 0; i < examsToCheck.Count(); i++)
            {
                if (givenExam.CourseId == examsToCheck[i].CourseId)
                {
                    if (givenExam.Moed == "a")
                        return givenExam.ExamDate < examsToCheck[i].ExamDate;
                    if(givenExam.Moed == "b")
                     return givenExam.ExamDate > examsToCheck[i].ExamDate;
                }  
            }
            return true;
            
        }
        public bool CheckAllMoeds(Exams givenExam, List<Exams> examsToCheck)
        {
            for (int i = 0; i < examsToCheck.Count(); i++)
            {
                if (givenExam.ExamDate == examsToCheck[i].ExamDate)
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckExamTimeClash(Exams givenExam, List<Exams> examsToCheck)
        {
            int i = 0;
            while (i < examsToCheck.Count())
            {

                if (examsToCheck[i].ExamDate == givenExam.ExamDate)
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
        public bool CheckLogin(string loggedUsername, string password)
        {
            //authenticates the login data. gets relevant record from the user db and checks if password is correct.
            List<User> objUsers = GetUserRecord(loggedUsername);
            if (objUsers.Any()) {
                User user = objUsers[0];
                return (user.Password.Equals(password));
            }
            return false;
            
        }
        public bool CheckResponsibility(string cid)
        {
            //checks if the user is a lecturer with ther targeted course within his responsibilty or if the user is a faculty member.
            List<string> courses = GetLectCourseQuery(Session["loggedUsername"].ToString());
            if ((courses.Contains(cid) && Session["permission"] == "2") || Session["permission"] == "3")
                return true;
            return false;
        }
        public bool CheckValues(string courseId, string studentName)
        {
            //if true - can update grade. else, can't.
            return CheckResponsibility(courseId) && CheckIfBelongsToClass(studentName, courseId);
        }
        public bool CheckIfBelongsToClass(string studentName, string courseId)
        {
            //gets studentname, course id and checks if the student exists in that course. if not null = true, else false.
            StudentCoursesDal coursesDal = new StudentCoursesDal();
            List<Courses> lectCourses = new List<Courses>();

            List<StudentCourses> studentCourses = new List<StudentCourses>();
            studentCourses.Add((from x in coursesDal.CoursesAndUsers
                                where x.Username.Equals(studentName) && x.CourseId.Equals(courseId)
                                select x).FirstOrDefault<StudentCourses>());
            
            return studentCourses[0] != null;
        }        
        public int CheckInputGrade(string givenInput)
        {
            //gets the grade as string and tries converting to int and checks if in range of 0-100. 
            //throws exception if it isn't a number, and returns -1 if given number is wrong.
            int grade;
            try
            {
                grade = Int32.Parse(givenInput);
                if (grade < 0 || grade > 100)
                    return -1;
                return grade;
            }
            catch (Exception)
            {
                Session["NoUpdate"] = "Wrong input as grade";
            }    
            return -1;
        }
    }
}
