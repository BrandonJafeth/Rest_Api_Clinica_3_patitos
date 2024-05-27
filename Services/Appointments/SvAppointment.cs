using Entities;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
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


        public async Task<List<DtoAppointment>> GetAppointmentsByUserId(int userId)
        {
            var appointments = await _myDbContext.Appointments
                .Include(x => x.User)
                .Include(x => x.Clinic_Branch)
                .Include(x => x.AppointmentType)
                .Where(x => x.User != null && x.User.Id_User == userId)
                .OrderBy(x => x.Date)
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

        public async Task<List<DtoAppointment>> GetAppointmentsForToday()
        {
            var today = DateTime.Today;


            var appointments = await _myDbContext.Appointments
                .Include(x => x.User)
                .Include(x => x.Clinic_Branch)
                .Include(x => x.AppointmentType)
                .Where(x => x.Date.Date == today)
                .ToListAsync();

            var dtoAppointments = new List<DtoAppointment>();

            foreach (var appointment in appointments)
            {
                var dtoAppointment = await ConvertToDto(appointment);
                dtoAppointments.Add(dtoAppointment);
            }

            return dtoAppointments;
        }

        public async Task<List<DateTime>> GetAppointmentDates()
        {
            var today = DateTime.Today;
            var sevenDaysFromNow = today.AddDays(7);

            var appointmentDates = await _myDbContext.Appointments
                .Where(x => x.Date.Date >= today && x.Date.Date <= sevenDaysFromNow)
                .Select(x => new DateTime(x.Date.Year, x.Date.Month, x.Date.Day, x.Date.Hour, x.Date.Minute, 0))
                .ToListAsync();

            return appointmentDates;
        }

  

        // WRITES

        public async Task<List<DtoAddAppointment>> AddAppointments(List<DtoAddAppointment> dtoAppointments)
        {


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
               .Where(x => x.Id_User == dtoAppointment.Id_User && x.Date.Date == dtoAppointment.Date.Date)
               .FirstOrDefaultAsync();


                if (lastAppointment != null)
                {
                    throw new Exception("You cannot create another appointment for the same user on the same day.");
                }

                var isDateTimeAvailable = await IsDateTimeAvailable(dtoAppointment.Date, dtoAppointment.Id_User);

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

                // Enviar correo electrónico con los detalles de la cita
                appointment = await _myDbContext.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Clinic_Branch)
                    .FirstOrDefaultAsync(a => a.Id_Appoitment == appointment.Id_Appoitment);

                //Dont change email , its the email that send the confirmation mail and have an app password 
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Cilinic Three Duckling", "paulafernandezmarchena031@gmail.com"));
                    message.To.Add(new MailboxAddress("", appointment.User.Email));
                    message.Subject = "Medical Appointment Confirmation";

                    var body = new TextPart("plain")
                    {
                        Text = $"Dear {appointment.User.User_Name},\n\n" +
                  $"Your appointment has been scheduled for {appointment.Date}.\n" +
                  $"Appoitment Details:\n" +
                  $"- Clinic Branch {appointment.Clinic_Branch.Branch_Name}\n" +
                  $"- Specialty: {appointment.AppointmentType.Name_type}\n\n" +
                  "Thanks for choosing us."
                    };

                    message.Body = body;


                    using (var client = new SmtpClient())
                    {

                        await client.ConnectAsync("smtp.gmail.com", 587, false);

                        await client.AuthenticateAsync("paulafernandezmarchena031@gmail.com", "srny jbgf flmg movi");

                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al enviar el correo electrónico .", ex);
                }
            }

            return addedAppointments;
        }

        public async Task<DtoAddAppointment> UpdateAppointment(int id, DtoAddAppointment dtoAppointment)
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

            var isDateTimeAvailable = await IsDateTimeAvailable(dtoAppointment.Date, dtoAppointment.Id_User);

            if (!isDateTimeAvailable)
            {
                throw new Exception("The selected date and time are not available.");
            }

            existingAppointment.Date = dtoAppointment.Date;

            await _myDbContext.SaveChangesAsync();

            return ConvertToAddDto(existingAppointment);
        }


        public async Task DeleteAppointment(int id)
        {

            var existingAppointment = await _myDbContext.Appointments.SingleOrDefaultAsync(x => x.Id_Appoitment == id);

            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            _myDbContext.Appointments.Remove(existingAppointment);
            await _myDbContext.SaveChangesAsync();
        }

        // PRIVATE METHODS for updating appointments
        private async Task<bool> IsDateTimeAvailable(DateTime date, int userId)
        {
            // Obtiene todas las citas para la misma fecha
            var appointmentsOnSameDay = await _myDbContext.Appointments
                .Where(x => x.Date.Date == date.Date && x.User.Id_User == userId)
                .ToListAsync();

            // Filtra las citas en memoria para encontrar las que están dentro de una hora de la hora proporcionada
            var overlappingAppointment = appointmentsOnSameDay
                .FirstOrDefault(x => Math.Abs((x.Date - date).TotalHours) < 1);

            return overlappingAppointment == null;
        }





        public async Task CancelAppointment(int id)
        {
     

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

        //Hola

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
