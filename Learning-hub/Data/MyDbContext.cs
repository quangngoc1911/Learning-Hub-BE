using Microsoft.EntityFrameworkCore;


namespace Learning_hub.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        #region DbSet
        public DbSet<DataCourse> DataCourses { get; set; }
        public DbSet<DataDocument> DataDocuments { get; set; }
        public DbSet<DataEducationProgram> DataEducationPrograms { get; set; }
        public DbSet<DataLesson> DataLessons { get; set; }
        public DbSet<DataRefreshToken> RefreshTokens { get; set; }
        public DbSet<DataRequestApproval> RequestApprovals { get; set; }
        //public DbSet<DataStudent> DataStudents { get; set; }
        public DbSet<DataTeacher> DataTeachers { get; set; }
        public DbSet<DataUser> DataUsers { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DataUser>(entity =>
            {
                entity.HasIndex(e => e.IdUser).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
                //entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                //entity.Property(e => e.CCCD).IsRequired().HasMaxLength(12);
            });
        }
    }
}
