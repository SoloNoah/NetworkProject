using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using proj.Models;
namespace proj.dal
{
    public class LectDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Lecturer>().ToTable("tblLect");
        }
        public DbSet<Lecturer> lectsCourses { get; set; }
    }
}