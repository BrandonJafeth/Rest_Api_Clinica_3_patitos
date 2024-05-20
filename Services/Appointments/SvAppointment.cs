using Entities;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _myDbContext.Appointments
                .Include(x => x.User)
                .Include(x => x.Clinic_Branch)
                .Include(x => x.AppointmentType)
                .ToListAsync();
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            return await _myDbContext.Appointments
                 .Include(x => x.User)
                 .Include(x => x.Clinic_Branch)
                 .Include(x => x.AppointmentType)
                 .SingleOrDefaultAsync(x => x.Id_Appoitment == id);
        }

        // WRITES
        public async Task<List<Appointment>> AddAppointments(List<Appointment> appointments)
        {
            // Check if any appointment already exists for the same user on the same day
            var existingAppointments = await _myDbContext.Appointments
                    .Where(x => appointments.Any(a => a.User.Id_User == x.User.Id_User && a.Date.Date == x.Date.Date))
                .ToListAsync();

            if (existingAppointments.Count > 0)
            {
                throw new Exception("You cannot create another appointment for the same user on the same day, please come back tomorrow..");
            }

            _myDbContext.Appointments.AddRangeAsync(appointments);
            _myDbContext.SaveChangesAsync();

            return appointments;
        }



        public async Task<Appointment> UpdateAppointment(int id, Appointment appointment)
        {
            var existingAppointment = await _myDbContext.Appointments.SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            if (!existingAppointment.Status)
            {
                throw new Exception("Only active appointments can be edited.");
            }

            var currentTime = DateTime.Now;
            var minimumCancellationTime = existingAppointment.Date.AddHours(-24);

            if (currentTime >= minimumCancellationTime)
            {
                throw new Exception("You cannot edit the appointment with less than 24 hours notice.");
            }

            // Check if the new date and time are available
            var isDateTimeAvailable = await IsDateTimeAvailable(appointment.Date);

            if (!isDateTimeAvailable)
            {
                throw new Exception("The selected date and time are not available.");
            }

            existingAppointment.Date = appointment.Date;

            
            if (appointment.Status == false)
            {
                existingAppointment.Status = false;
            }

            _myDbContext.SaveChanges();

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
    }
}
