using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class faMemeberDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ExamsDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<faMemeberDal>().ToTable("tblfaMembers");
        }
        public DbSet<faMember> exams { get; set; }
    }
}
