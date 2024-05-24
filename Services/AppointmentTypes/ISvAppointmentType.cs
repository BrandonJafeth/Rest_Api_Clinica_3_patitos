using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.AppointmentTypes
{
    public interface ISvAppointmentType
    {
        public Task<List<DtoAppointmentType>> GetAllAppointmentType();
        public Task<DtoAppointmentType> GetAppointmentTypeById(int id);
    }

}
