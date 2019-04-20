﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyJournal.Domain.Data;

namespace MyJournal.Domain.Migrations
{
    [DbContext(typeof(MyJournalDbContext))]
    partial class MyJournalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyJournal.Domain.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<int?>("GroupId");

                    b.Property<string>("LastName");

                    b.Property<string>("Login");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<string>("Role");

                    b.Property<string>("Surname");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("ApplicationUser");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ApplicationUser");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Attend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LessonId");

                    b.Property<int?>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("LessonId");

                    b.HasIndex("StudentId");

                    b.ToTable("Attends");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Letter");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<int?>("GroupId");

                    b.Property<int?>("SubjectId");

                    b.Property<int?>("TeacherId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Mark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AttendId");

                    b.Property<int>("Grade");

                    b.Property<DateTime>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("AttendId");

                    b.ToTable("Marks");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddresseeId");

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("Read");

                    b.Property<int?>("SenderId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AddresseeId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int?>("TeacherId");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Student", b =>
                {
                    b.HasBaseType("MyJournal.Domain.Entities.ApplicationUser");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Teacher", b =>
                {
                    b.HasBaseType("MyJournal.Domain.Entities.ApplicationUser");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.ApplicationUser", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Attend", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId");

                    b.HasOne("MyJournal.Domain.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Lesson", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("MyJournal.Domain.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.HasOne("MyJournal.Domain.Entities.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Mark", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.Attend", "Attend")
                        .WithMany()
                        .HasForeignKey("AttendId");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Message", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.ApplicationUser", "Addressee")
                        .WithMany()
                        .HasForeignKey("AddresseeId");

                    b.HasOne("MyJournal.Domain.Entities.Teacher", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("MyJournal.Domain.Entities.Subject", b =>
                {
                    b.HasOne("MyJournal.Domain.Entities.Teacher")
                        .WithMany("Subjects")
                        .HasForeignKey("TeacherId");
                });
#pragma warning restore 612, 618
        }
    }
}
