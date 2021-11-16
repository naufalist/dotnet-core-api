﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentRestAPI.Models;

namespace StudentRestAPI.Migrations
{
    [DbContext(typeof(StudentDbContext))]
    [Migration("20211116104044_InitialMigrate")]
    partial class InitialMigrate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("StudentRestAPI.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<decimal>("IPK")
                        .HasColumnType("decimal(4,2)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Student");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Zaky",
                            IPK = 3.50m,
                            LastName = "Ramadhan"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Devina",
                            IPK = 4.00m,
                            LastName = "Ramadhani"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Putri",
                            IPK = 3.35m,
                            LastName = "Larasati"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
