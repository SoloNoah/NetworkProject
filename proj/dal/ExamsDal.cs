using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class ExamsDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ExamsDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Exams>().ToTable("tblDeptExams");
        }
        public DbSet<Exams> exams { get; set; }
    }
}