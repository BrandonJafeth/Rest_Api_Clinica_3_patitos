using Entities;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.AppointmentTypes
{
    public class SvAppointmentType : ISvAppointmentType
    {
        private readonly MyContext _myDbContext;

        public SvAppointmentType(MyContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task<DtoAppointmentType> GetAppointmentTypeById(int id)
        {
            var appointmentType = await _myDbContext.AppointmentTypes
                .SingleOrDefaultAsync(x => x.Id_Appoitment_Type == id);

            return MapToDto(appointmentType);
        }

        public async Task<List<DtoAppointmentType>> GetAllAppointmentType()
        {
            var appointmentTypes = await _myDbContext.AppointmentTypes.ToListAsync();

            return appointmentTypes.Select(MapToDto).ToList();
        }

        private DtoAppointmentType MapToDto(AppointmentType appointmentType)
        {
            return new DtoAppointmentType
            {
                Id_Appoitment_Type = appointmentType.Id_Appoitment_Type,
                Name_type = appointmentType.Name_type
            };
        }

    }

    }
