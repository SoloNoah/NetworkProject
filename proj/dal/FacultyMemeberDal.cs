using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class FacultyMemeberDal: DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<FacultyMemeberDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FacultyMember>().ToTable("tblFacultyMembers");
        }
        public DbSet<FacultyMember> facultyData { get; set; }
    }
}
