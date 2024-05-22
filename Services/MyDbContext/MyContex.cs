using Entities;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Contex medio configurado
namespace Services.MyDbContext
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Clinic_Branch> Clinic_Branches { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.Id_Appoitment);

            modelBuilder.Entity<AppointmentType>()
                .HasKey(a => a.Id_Appoitment_Type);

            modelBuilder.Entity<Clinic_Branch>()
                .HasKey(a => a.Id_ClinicBranch);

            modelBuilder.Entity<User>()
                .HasKey(a => a.Id_User);

            modelBuilder.Entity<Rol>()
                .HasKey(a => a.Id_Rol);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Users)
                .WithOne(e => e.Rol)
                .HasForeignKey(e => e.Id_Rol)
                .IsRequired(false);

            modelBuilder.Entity<Clinic_Branch>().HasData(
                new Clinic_Branch { Id_ClinicBranch = 1, Branch_Name = "Sucursal Principal" }
            );

            modelBuilder.Entity<AppointmentType>().HasData(
                new AppointmentType { Id_Appoitment_Type = 1, Name_type = "Tipo de Cita Regular" }
            );

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id_Rol = 1, Name_Rol = "USER" }
            );


            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.User)
                .WithMany(user => user.Appointments)
                .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, their appointments are also deleted

            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.Clinic_Branch)
                .WithMany(clinicBranch => clinicBranch.Appointments)
                .OnDelete(DeleteBehavior.Cascade); // If a branch is deleted, its appointments are also deleted

            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.AppointmentType)
                .WithMany(appointmentType => appointmentType.Appointments)
                .OnDelete(DeleteBehavior.Cascade); // If an appointment type is deleted, its appointments are also deleted
        }
    }
}
