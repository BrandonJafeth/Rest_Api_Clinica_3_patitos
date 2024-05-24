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
                .OrderByDescending(x => x.Id_Appoitment)
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


        public async Task<List<DtoAppointment>> GetAppointmentsByUserName(string User_name)
        {
            var appointments = await _myDbContext.Appointments
                .Include(x => x.User)
                .Include(x => x.Clinic_Branch)
                .Include(x => x.AppointmentType)
                .Where(x => x.User != null && x.User.User_Name == User_name)
                .ToListAsync();

            if (appointments.Count == 0)
            {
                throw new Exception("No appointments found for the specified user.");

            }

            var dtoAppointments = new List<DtoAppointment>();

            foreach (var appointment in appointments)
            {
                var dtoAppointment = await ConvertToDto(appointment);
                dtoAppointments.Add(dtoAppointment);
            }

            return dtoAppointments;
        }


        // WRITES

        public async Task<List<DtoAddAppointment>> AddAppointments(List<DtoAddAppointment> dtoAppointments, string role)
        {
            if (role != "USER")
            {
                throw new Exception("Only users with role USER can create appointments.");
            }

            var addedAppointments = new List<DtoAddAppointment>();

            foreach (var dtoAppointment in dtoAppointments)
            {
                var user = await _myDbContext.Users.FindAsync(dtoAppointment.Id_User);
                var clinicBranch = await _myDbContext.Clinic_Branches.FindAsync(dtoAppointment.Id_ClinicBranch);
                var appointmentType = await _myDbContext.AppointmentTypes.FindAsync(dtoAppointment.Id_Appoitment_Type);

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

                var lastAppointment = await _myDbContext.Appointments
               .Where(x => x.Id_User == dtoAppointment.Id_User && x.Date == dtoAppointment.Date)
               .FirstOrDefaultAsync();


                if (lastAppointment != null)
                {
                    throw new Exception("You cannot create another appointment for the same user on the same day.");
                }

                var isDateTimeAvailable = await IsDateTimeAvailable(dtoAppointment.Date);
                if (!isDateTimeAvailable)
                {
                    throw new Exception("The selected date and time are not available.");
                }

                var appointment = new Appointment
                {
                    Status = dtoAppointment.Status,
                    Date = dtoAppointment.Date,
                    User = user,
                    Clinic_Branch = clinicBranch,
                    AppointmentType = appointmentType
                };

                _myDbContext.Appointments.Add(appointment);
                await _myDbContext.SaveChangesAsync();

                dtoAppointment.Id_Appointment = appointment.Id_Appoitment;
                addedAppointments.Add(dtoAppointment);
            }

            return addedAppointments;
        }

        public async Task<DtoAddAppointment> UpdateAppointment(int id, DtoAddAppointment dtoAppointment, string role)
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

            existingAppointment.Date = dtoAppointment.Date;

            var clinicBranch = await _myDbContext.Clinic_Branches.FindAsync(dtoAppointment.Id_ClinicBranch);
            if (clinicBranch == null)
            {
                throw new Exception("Clinic branch not found.");
            }
            existingAppointment.Clinic_Branch = clinicBranch;

            var appointmentType = await _myDbContext.AppointmentTypes.FindAsync(dtoAppointment.Id_Appoitment_Type);
            if (appointmentType == null)
            {
                throw new Exception("Appointment type not found.");
            }
            existingAppointment.AppointmentType = appointmentType;

            var user = await _myDbContext.Users.FindAsync(dtoAppointment.Id_User);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            existingAppointment.User = user;

            if (dtoAppointment.Status == false)
            {
                existingAppointment.Status = false;
            }

            var isDateTimeAvailable = await IsDateTimeAvailable(dtoAppointment.Date);
            if (!isDateTimeAvailable)
            {
                throw new Exception("The selected date and time are not available.");
            }

            existingAppointment.Date = dtoAppointment.Date;

            await _myDbContext.SaveChangesAsync();

            return ConvertToAddDto(existingAppointment);
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


        public DtoAddAppointment ConvertToAddDto(Appointment appointment)
        {

            if (appointment == null)
            {
                throw new Exception("La cita es nula.");
            }

            if (appointment.Clinic_Branch == null)
            {
                throw new Exception("La sucursal de la clínica es nula.");
            }

            if (appointment.AppointmentType == null)
            {
                throw new Exception("El tipo de cita es nulo.");
            }

            if (appointment.User == null)
            {
                throw new Exception("El usuario es nulo.");
            }

            return new DtoAddAppointment
            {
                Id_Appointment = appointment.Id_Appoitment,
                Status = appointment.Status,
                Date = DateTime.Parse(appointment.Date.ToString("yyyy-MM-dd HH:mm")),
                Id_ClinicBranch = appointment.Clinic_Branch.Id_ClinicBranch,
                Id_Appoitment_Type = appointment.AppointmentType.Id_Appoitment_Type,
                Id_User = appointment.User.Id_User
            };
        }






    }
}
