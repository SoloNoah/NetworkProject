using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class LecturerGotStudsDal :DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<LecturerGotStudsDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable("tblAllStudents");
        }
        public DbSet<Student> lectsStudents { get; set; }
    }
}