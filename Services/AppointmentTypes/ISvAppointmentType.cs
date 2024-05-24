using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentTypes
{
    public interface ISvAppointmentType
    {
        public Task<List<AppointmentType>> GetAllAppointmentType();
        public Task<AppointmentType> GetAppointmentTypeById(int id);

        public Task<List<AppointmentType>> AddAppointmentType(List<AppointmentType> appointmentTypes);
        public Task<AppointmentType> UpdateAppointmentType(int id, AppointmentType appointmentType);
        public Task DeleteAppointmentType(int id);
      
    }
}
