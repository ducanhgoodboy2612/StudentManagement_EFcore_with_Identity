using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using StudentManageApp_Codef.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace StudentManageApp_Codef.Data
{
    public class AppDbContext : IdentityDbContext
    {
        
        public DbSet<Department> Departments_n { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TuitionFee> TuitionFees { get; set; }
        public DbSet<Test> Test { get; set; }

        public DbSet<User> Users { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // add index
            modelBuilder.Entity<Student>()
                .HasIndex(s => new { s.FirstName, s.LastName, s.Phone })
                .HasDatabaseName("idx_student_search"); 
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseOracle(DBConnect.GetConnectionString());
        //    }
        //}
    }
}
