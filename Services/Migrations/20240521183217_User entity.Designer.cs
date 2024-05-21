﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.MyDbContext;

#nullable disable

namespace Services.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20240521183217_User entity")]
    partial class Userentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Appointment", b =>
                {
                    b.Property<int>("Id_Appoitment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Appoitment"));

                    b.Property<int>("AppointmentTypeId_Appoitment_Type")
                        .HasColumnType("int");

                    b.Property<int>("Clinic_BranchId_ClinicBranch")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Appoitment_Type")
                        .HasColumnType("int");

                    b.Property<int>("Id_ClinicBranch")
                        .HasColumnType("int");

                    b.Property<int>("Id_User")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId_User")
                        .HasColumnType("int");

                    b.HasKey("Id_Appoitment");

                    b.HasIndex("AppointmentTypeId_Appoitment_Type");

                    b.HasIndex("Clinic_BranchId_ClinicBranch");

                    b.HasIndex("UserId_User");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Entities.AppointmentType", b =>
                {
                    b.Property<int>("Id_Appoitment_Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Appoitment_Type"));

                    b.Property<string>("Name_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Appoitment_Type");

                    b.ToTable("AppointmentTypes");
                });

            modelBuilder.Entity("Entities.Clinic_Branch", b =>
                {
                    b.Property<int>("Id_ClinicBranch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_ClinicBranch"));

                    b.Property<string>("Branch_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_ClinicBranch");

                    b.ToTable("Clinic_Branches");
                });

            modelBuilder.Entity("Entities.Rol", b =>
                {
                    b.Property<int>("Id_Rol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Rol"));

                    b.Property<string>("Name_Rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Rol");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Property<int>("Id_User")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_User"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Id_Rol")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RolId_Rol")
                        .HasColumnType("int");

                    b.Property<string>("User_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_User");

                    b.HasIndex("RolId_Rol");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Entities.Appointment", b =>
                {
                    b.HasOne("Entities.AppointmentType", "AppointmentType")
                        .WithMany("Appointments")
                        .HasForeignKey("AppointmentTypeId_Appoitment_Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Clinic_Branch", "Clinic_Branch")
                        .WithMany("Appointments")
                        .HasForeignKey("Clinic_BranchId_ClinicBranch")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.User", "User")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppointmentType");

                    b.Navigation("Clinic_Branch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.HasOne("Entities.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolId_Rol");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Entities.AppointmentType", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Entities.Clinic_Branch", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
