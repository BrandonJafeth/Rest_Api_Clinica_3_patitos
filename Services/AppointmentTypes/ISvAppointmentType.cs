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
      
    }
}
