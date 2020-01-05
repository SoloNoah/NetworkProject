using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class StudentCoursesDal:DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<StudentCoursesDal>(null);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentCourses>().ToTable("tblStudentCourses");
        }
        public DbSet<StudentCourses> CoursesAndUsers { get; set; }
    }
}