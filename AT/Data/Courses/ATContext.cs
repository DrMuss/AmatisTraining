using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Courses
{
    public class ATContext : DbContext
    {
        public ATContext(DbContextOptions<ATContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
       
        public DbSet<UserCourse> UserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>()
                .HasKey(o => new {o.UserId, o.CourseId });
        }

    }
}
