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


        public async Task<List<AppointmentType>> GetAllAppointmentType()
        {
            return await _myDbContext.AppointmentTypes.ToListAsync();
        }
    }
}
