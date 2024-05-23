using Entities;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.Appointments
{
    public class SvAppointment : ISvAppointment
    {
        private readonly MyContext _myDbContext;

        public SvAppointment(MyContext myDbContext)
        {
            _myDbContext = myDbContext;
        }


        // READS
        public async Task<List<DtoAppointment>> GetAllAppointments()
        {
            var appointments = await _myDbContext.Appointments
                .Include(x => x.User)
                .Include(x => x.Clinic_Branch)
                .Include(x => x.AppointmentType)
                .ToListAsync();

            var dtoAppointments = new List<DtoAppointment>();

            foreach (var appointment in appointments)
            {
                var dtoAppointment = await ConvertToDto(appointment);
                dtoAppointments.Add(dtoAppointment);
            }

            return dtoAppointments;
        }


        public async Task<DtoAppointment> GetAppointmentById(int id)
        {
            var appointment = await _myDbContext.Appointments
                 .Include(x => x.User)
                 .Include(x => x.Clinic_Branch)
                 .Include(x => x.AppointmentType)
                 .SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            return await ConvertToDto(appointment);
        }




        // WRITES

        public async Task<List<Appointment>> AddAppointments(List<Appointment> appointments, string role)
        {
            if (role != "USER")
            {
                throw new Exception("Only users with role USER can create appointments.");
            }

            foreach (var appointment in appointments)
            {
                var user = await _myDbContext.Users.FindAsync(appointment.Id_User);
                var clinicBranch = await _myDbContext.Clinic_Branches.FindAsync(appointment.Id_ClinicBranch);
                var appointmentType = await _myDbContext.AppointmentTypes.FindAsync(appointment.Id_Appoitment_Type);

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                if (clinicBranch == null)
                {
                    throw new Exception("Clinic branch not found.");
                }

                if (appointmentType == null)
                {
                    throw new Exception("Appointment type not found.");
                }

                // Buscar la última cita del usuario
                var lastAppointment = await _myDbContext.Appointments
                    .Where(x => x.Id_User == appointment.Id_User && x.Date.Date == appointment.Date.Date)
                    .FirstOrDefaultAsync();

                // Comprobar si ha pasado suficiente tiempo desde la última cita
                if (lastAppointment != null)
                {
                    throw new Exception("You cannot create another appointment for the same user on the same day.");
                }

                // Asignar el usuario, la clínica y el tipo de cita a la cita
                appointment.User = user;
                appointment.Clinic_Branch = clinicBranch;
                appointment.AppointmentType = appointmentType;

                _myDbContext.Appointments.Add(appointment);
                await _myDbContext.SaveChangesAsync();
            }

            return appointments;
        }



        public async Task<Appointment> UpdateAppointment(int id, Appointment appointment, string role)
        {
            if (role != "USER")
            {
                throw new Exception("Only users with role USER can update appointments.");
            }

            var existingAppointment = await _myDbContext.Appointments.SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            if (!existingAppointment.Status)
            {
                throw new Exception("Only active appointments can be edited.");
            }

            // Check if the new date and time are available
            var isDateTimeAvailable = await IsDateTimeAvailable(appointment.Date);

            if (!isDateTimeAvailable)
            {
                throw new Exception("The selected date and time are not available.");
            }

            existingAppointment.Date = appointment.Date;

            var clinicBranch = await _myDbContext.Clinic_Branches.FindAsync(appointment.Id_ClinicBranch);
            if (clinicBranch == null)
            {
                throw new Exception("Clinic branch not found.");
            }
            existingAppointment.Clinic_Branch = clinicBranch;

            var appointmentType = await _myDbContext.AppointmentTypes.FindAsync(appointment.Id_Appoitment_Type);
            if (appointmentType == null)
            {
                throw new Exception("Appointment type not found.");
            }
            existingAppointment.AppointmentType = appointmentType;

            if (appointment.Status == false)
            {
                existingAppointment.Status = false;
            }

            await _myDbContext.SaveChangesAsync();

            return existingAppointment;
        }


        public async Task DeleteAppointment(int id, string role)
        {
            if (role != "admin")
            {
                throw new Exception("Only admin users can delete appointments.");
            }

            var existingAppointment = await _myDbContext.Appointments.SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            _myDbContext.Appointments.Remove(existingAppointment);
            await _myDbContext.SaveChangesAsync();
        }

        // PRIVATE METHODS for updating appointments
        private async Task<bool> IsDateTimeAvailable(DateTime date)
        {
            // Check if any appointment already exists for the same date
            var existingAppointment = await _myDbContext.Appointments
                .SingleOrDefaultAsync(x => x.Date.Date == date.Date);

            return existingAppointment == null;
        }


        public async Task CancelAppointment(int id, string role)
        {
            if (role != "USER")
            {
                throw new Exception("Only users with role USER can cancel appointments.");
            }

            var existingAppointment = await _myDbContext.Appointments.SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            var currentTime = DateTime.Now;
            var minimumCancellationTime = existingAppointment.Date.AddHours(-24);

            if (currentTime >= minimumCancellationTime)
            {
                throw new Exception("You cannot cancel the appointment with less than 24 hours notice.");
            }

            existingAppointment.Status = false;

            await _myDbContext.SaveChangesAsync();
        }


        public async Task<DtoAppointment> ConvertToDto(Appointment appointment)
        {
            return new DtoAppointment
            {
                Id_Appointment = appointment.Id_Appoitment,
                Name_type = appointment.AppointmentType.Name_type,
                Branch_Name = appointment.Clinic_Branch.Branch_Name,
                Status = appointment.Status,
                Date = DateTime.Parse(appointment.Date.ToString("yyyy-MM-dd HH:mm")),
                User_Name = appointment.User.User_Name
            };
        }





    }
}
