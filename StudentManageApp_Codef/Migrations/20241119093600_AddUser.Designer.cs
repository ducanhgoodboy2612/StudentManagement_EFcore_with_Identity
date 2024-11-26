﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using StudentManageApp_Codef.Data;

#nullable disable

namespace StudentManageApp_Codef.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241119093600_AddUser")]
    partial class AddUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceID"));

                    b.Property<DateTime>("AttendanceDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("ClassID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("StudentID")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("AttendanceID");

                    b.HasIndex("ClassID");

                    b.HasIndex("StudentID");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Class", b =>
                {
                    b.Property<int>("ClassID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassID"));

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("CourseID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("LecturerID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Year")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("ClassID");

                    b.HasIndex("CourseID");

                    b.HasIndex("LecturerID");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Credits")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("CourseID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments_n");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentID"));

                    b.Property<int>("ClassID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("StudentID")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("EnrollmentID");

                    b.HasIndex("ClassID");

                    b.HasIndex("StudentID");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Exam", b =>
                {
                    b.Property<int>("ExamID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExamID"));

                    b.Property<int>("ClassID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime?>("ExamDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("ExamType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<decimal>("TotalMarks")
                        .HasColumnType("DECIMAL(18, 2)");

                    b.HasKey("ExamID");

                    b.HasIndex("ClassID");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Grade", b =>
                {
                    b.Property<int>("GradeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GradeID"));

                    b.Property<int>("EnrollmentID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("ExamID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<decimal>("MarksObtained")
                        .HasColumnType("DECIMAL(18, 2)");

                    b.Property<int?>("StudentID")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("GradeID");

                    b.HasIndex("EnrollmentID");

                    b.HasIndex("ExamID");

                    b.HasIndex("StudentID");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Lecturer", b =>
                {
                    b.Property<int>("LecturerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LecturerID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int?>("YoB")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("LecturerID");

                    b.ToTable("Lecturers");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleID"));

                    b.Property<int>("ClassID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("ScheduleID");

                    b.HasIndex("ClassID")
                        .IsUnique();

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Student", b =>
                {
                    b.Property<int>("StudentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<int>("Gender")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("StudentID");

                    b.HasIndex("FirstName", "LastName", "Phone")
                        .HasDatabaseName("idx_student_search");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Test", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("ID");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.TuitionFee", b =>
                {
                    b.Property<int>("TuitionFeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TuitionFeeID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("DECIMAL(18, 2)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("StudentID")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Year")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("TuitionFeeID");

                    b.HasIndex("StudentID");

                    b.ToTable("TuitionFees");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Attendance", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Class", "Class")
                        .WithMany("Attendances")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManageApp_Codef.Data.Models.Student", "Student")
                        .WithMany("Attendances")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Class", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Course", "Course")
                        .WithMany("Classes")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManageApp_Codef.Data.Models.Lecturer", "Lecturer")
                        .WithMany("Classes")
                        .HasForeignKey("LecturerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Course", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Enrollment", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Class", "Class")
                        .WithMany("Enrollments")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManageApp_Codef.Data.Models.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Exam", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Class", "Class")
                        .WithMany("Exams")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Grade", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Enrollment", "Enrollment")
                        .WithMany("Grades")
                        .HasForeignKey("EnrollmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManageApp_Codef.Data.Models.Exam", "Exam")
                        .WithMany("Grades")
                        .HasForeignKey("ExamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManageApp_Codef.Data.Models.Student", null)
                        .WithMany("Grades")
                        .HasForeignKey("StudentID");

                    b.Navigation("Enrollment");

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Schedule", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Class", "Class")
                        .WithOne("Schedule")
                        .HasForeignKey("StudentManageApp_Codef.Data.Models.Schedule", "ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.TuitionFee", b =>
                {
                    b.HasOne("StudentManageApp_Codef.Data.Models.Student", "Student")
                        .WithMany("TuitionFees")
                        .HasForeignKey("StudentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Class", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Enrollments");

                    b.Navigation("Exams");

                    b.Navigation("Schedule")
                        .IsRequired();
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Course", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Enrollment", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Exam", b =>
                {
                    b.Navigation("Grades");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Lecturer", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("StudentManageApp_Codef.Data.Models.Student", b =>
                {
                    b.Navigation("Attendances");

                    b.Navigation("Enrollments");

                    b.Navigation("Grades");

                    b.Navigation("TuitionFees");
                });
#pragma warning restore 612, 618
        }
    }
}
