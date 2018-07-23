using Microsoft.EntityFrameworkCore;
using UvpClient.Models;

namespace UvpClient.Design {
    public class DesignDataContext : DbContext {
        public DbSet<Student> Student { get; set; }
        public DbSet<Clazz> Clazz { get; set; }
        public DbSet<TeachingClazz> TeachingClazz { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Homework> Homework { get; set; }
        public DbSet<StudentAssignment> StudentAssignment { get; set; }
        public DbSet<GroupAssignment> GroupAssignment { get; set; }
        public DbSet<Questionnaire> Questionnaire { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<PrivacyData> PrivacyData { get; set; }

        public DbSet<PeerWorkGroupEvaluation> PeerWorkGroupEvaluation {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Student>().HasIndex(m => m.StudentId);
            builder.Entity<Student>().HasOne(m => m.TeachingClazz)
                .WithMany(m => m.Students).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StudentAssignment>()
                .HasKey(m => new {m.HomeworkID, m.StudentID});
            builder.Entity<StudentAssignment>().HasIndex(m => m.HomeworkID);
            builder.Entity<StudentAssignment>().HasIndex(m => m.StudentID);
            builder.Entity<GroupAssignment>()
                .HasKey(m => new {m.HomeworkID, m.GroupID});
            builder.Entity<GroupAssignment>().HasIndex(m => m.HomeworkID);
            builder.Entity<GroupAssignment>().HasIndex(m => m.GroupID);
            builder.Entity<Vote>()
                .HasKey(m => new {m.QuestionnaireID, m.StudentID});
            builder.Entity<PeerWorkGroupEvaluation>().HasOne(m => m.Target)
                .WithMany(m => m.PeerWorkGroupEvaluations)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseInMemoryDatabase("DesignDatabase");

            base.OnConfiguring(optionsBuilder);
        }
    }
}