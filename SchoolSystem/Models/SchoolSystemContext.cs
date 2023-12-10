using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchoolSystem.Models;

namespace SchoolSystem;

public partial class SchoolSystemContext : DbContext
{
    public SchoolSystemContext()
    {
    }

    public SchoolSystemContext(DbContextOptions<SchoolSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentPosition> DepartmentPositions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<SchoolClass> SchoolClasses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.Property(e => e.CourseName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FkEmployeeId).HasColumnName("FK_EmployeeId");

            entity.HasOne(d => d.FkEmployee).WithMany(p => p.Courses)
                .HasForeignKey(d => d.FkEmployeeId)
                .HasConstraintName("FK_Course_Employee");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DepartmentPosition>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DepartmentPosition");

            entity.Property(e => e.FkDepartmentId).HasColumnName("FK_DepartmentId");
            entity.Property(e => e.FkPositionId).HasColumnName("FK_PositionId");

            entity.HasOne(d => d.FkDepartment).WithMany()
                .HasForeignKey(d => d.FkDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentPosition_Department");

            entity.HasOne(d => d.FkPosition).WithMany()
                .HasForeignKey(d => d.FkPositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentPosition_Position");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FkDepartment).HasColumnName("FK_Department");
            entity.Property(e => e.FkPosition).HasColumnName("FK_Position");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PersonalNumber)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(6, 0)");

            entity.HasOne(d => d.FkDepartmentNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.FkDepartment)
                .HasConstraintName("FK_Employee_Department");

            entity.HasOne(d => d.FkPositionNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.FkPosition)
                .HasConstraintName("FK_Employee_Position");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grade");

            entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseId");
            entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentId");

            entity.HasOne(d => d.FkCourse).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FkCourseId)
                .HasConstraintName("FK_Grade_Course");

            entity.HasOne(d => d.FkStudent).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FkStudentId)
                .HasConstraintName("FK_Grade_Student");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.FkDepartmentId).HasColumnName("FK_DepartmentId");
            entity.Property(e => e.PositionName)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SchoolClass>(entity =>
        {
            entity.ToTable("SchoolClass");

            entity.Property(e => e.ClassName)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.EmailAdress).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FkSchoolClassId).HasColumnName("FK_SchoolClassId");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PersonalNumber)
                .HasMaxLength(13)
                .IsUnicode(false);

            entity.HasOne(d => d.FkSchoolClass).WithMany(p => p.Students)
                .HasForeignKey(d => d.FkSchoolClassId)
                .HasConstraintName("FK_Student_SchoolClass");
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("StudentCourse");

            entity.Property(e => e.FkCourseId).HasColumnName("FK_CourseId");
            entity.Property(e => e.FkStudentId).HasColumnName("FK_StudentId");

            entity.HasOne(d => d.FkCourse).WithMany()
                .HasForeignKey(d => d.FkCourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentCourse_Course");

            entity.HasOne(d => d.FkStudent).WithMany()
                .HasForeignKey(d => d.FkStudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentCourse_Student");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
