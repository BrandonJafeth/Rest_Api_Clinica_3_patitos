using Entities;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            modelBuilder.Entity<User>()
                .HasOne(user => user.Rol)
                .WithMany(rol => rol.Users)
                .OnDelete(DeleteBehavior.Cascade); // If a role is deleted, its users are also deleted

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
