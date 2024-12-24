using Doctors_Web_Forum.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctors_Web_Forum.DAL.Data
{
    public class DataDBContext : IdentityDbContext<User>
    {
        public DataDBContext(DbContextOptions<DataDBContext> options) : base(options) { }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<DoctorProfile> DoctorProfiles { get; set; }

        public DbSet<LoginAttempt> LoginAttempts { get; set; }

        

        public DbSet<AdminLog> AdminLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình mối quan hệ giữa User và Question
            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)  // Mỗi câu hỏi có một người dùng
                .WithMany(u => u.Questions)  // Mỗi người dùng có nhiều câu hỏi
                .HasForeignKey(q => q.UserId)  // Khóa ngoại là UserId trong bảng Question
                .OnDelete(DeleteBehavior.Restrict);  // Khi xóa người dùng, không xóa câu hỏi

            // Cấu hình mối quan hệ giữa Question và Answer
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)  // Mỗi câu trả lời thuộc về một câu hỏi
                .WithMany(q => q.Answers)  // Mỗi câu hỏi có nhiều câu trả lời
                .HasForeignKey(a => a.QuestionId)  // Khóa ngoại là QuestionId trong bảng Answer
                .OnDelete(DeleteBehavior.Restrict);  // Khi xóa câu hỏi, không xóa câu trả lời

            // Cấu hình mối quan hệ giữa User và Answer
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)  // Mỗi câu trả lời có một người dùng
                .WithMany(u => u.Answers)  // Mỗi người dùng có nhiều câu trả lời
                .HasForeignKey(a => a.UserId)  // Khóa ngoại là UserId trong bảng Answer
                .OnDelete(DeleteBehavior.Restrict);  // Khi xóa người dùng, không xóa câu trả lời

            // Cấu hình mối quan hệ 1-1 giữa Profile và User
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.User)        // Mỗi Profile có một User
                .WithOne(u => u.Profile)    // Mỗi User có một Profile
                .HasForeignKey<Profile>(p => p.UserId)  // Khóa ngoại là UserId trong bảng Profile
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
