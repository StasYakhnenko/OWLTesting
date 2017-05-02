using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.DB;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL
{
    public class MainContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MainContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TeacherTest>()
           .HasKey(t => new { t.TestId, t.UserId });

            modelBuilder.Entity<TeacherTest>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.TeacherTests)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<TeacherTest>()
                .HasOne(pt => pt.Test)
                .WithMany(t => t.TeacherTests)
                .HasForeignKey(pt => pt.TestId);
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<GivenQuestion> GivenQuestions { get; set; }
        public DbSet<GivenAnswer> GivenAnswers { get; set; }
    }
}
