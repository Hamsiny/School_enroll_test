using Microsoft.EntityFrameworkCore;
using UxtrataTask.Models;


namespace UxtrataTask.Context;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseSelection> CourseSelections { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseSelection>()
            .HasKey(cs => new { cs.ID });

        modelBuilder.Entity<CourseSelection>()
            .HasOne(cs => cs.Student)
            .WithMany(s => s.CourseSelections)
            .HasForeignKey(cs => cs.StudentID);

        modelBuilder.Entity<CourseSelection>()
            .HasOne(cs => cs.Course)
            .WithMany(c => c.CourseSelections)
            .HasForeignKey(cs => cs.CourseID);
    }
}