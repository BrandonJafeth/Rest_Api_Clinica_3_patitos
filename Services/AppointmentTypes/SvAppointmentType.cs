using Entities;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentTypes
{
    public class SvAppointmentType : ISvAppointmentType
    {
        private readonly MyContext _myDbContext;

        public SvAppointmentType(MyContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task<AppointmentType> GetAppointmentTypeById(int id)
        {
            return await _myDbContext.AppointmentTypes
                .Include(x => x.Appointments)
                .SingleOrDefaultAsync(x => x.Id_Appoitment_Type== id);
        }
        public async Task<List<AppointmentType>> AddAppointmentType(List<AppointmentType> appointmentTypes)
        {

          return await _myDbContext.AppointmentTypes.ToListAsync();
        }

        public async Task DeleteAppointmentType(int id)
        {

            var appointmentType = await _myDbContext.AppointmentTypes.FindAsync(id);

            if (appointmentType == null)
            {
                throw new Exception("Appointment type not found.");
            }

            var hasAssociatedAppointments = await _myDbContext.Appointments.AnyAsync(x => x.Id_Appoitment_Type == id);

            if (hasAssociatedAppointments)
            {
                throw new Exception("Cannot delete appointment type as it has associated appointments.");
            }

            _myDbContext.AppointmentTypes.Remove(appointmentType);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<List<AppointmentType>> GetAllAppointmentType()
        {
            return await _myDbContext.AppointmentTypes.ToListAsync();
        }

        public async Task<AppointmentType> UpdateAppointmentType(int id, AppointmentType appointmentType)
        {
            var existingAppointmentType = await _myDbContext.AppointmentTypes.FindAsync(id);

            if (existingAppointmentType == null)
            {
                throw new Exception("Appointment type not found.");
            }

            var hasAssociatedAppointments = await _myDbContext.Appointments.AnyAsync(x => x.Id_Appoitment_Type == id);

            if (hasAssociatedAppointments)
            {
                throw new Exception("Cannot update appointment type as it has associated appointments.");
            }

            existingAppointmentType.Name_type = appointmentType.Name_type;

            await _myDbContext.SaveChangesAsync();

            return existingAppointmentType;
        }



        //private async Task<bool> IsDateTimeAvailable(DateTime date)
        //{
        //    var existingAppointment = await _myDbContext.Appointments
        //        .SingleOrDefaultAsync(x => x.Date.Date == date.Date);

        //    return existingAppointment == null;
        //}
    }
}
