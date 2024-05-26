using Entities;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.Appointments
{
    public interface ISvAppointment
    {
        //READS 
        public Task<List<DtoAppointment>> GetAllAppointments();
        public Task<DtoAppointment> GetAppointmentById(int id);

        public Task<List<DtoAppointment>> GetAppointmentsByUserId(int UserId);

        Task<List<DtoAppointment>> GetAppointmentsForToday();


        public Task<List<DateTime>> GetAppointmentDates();





        //WRITES
        public Task<List<DtoAddAppointment>> AddAppointments(List<DtoAddAppointment> appointments);
        public Task<DtoAddAppointment> UpdateAppointment(int id, DtoAddAppointment appointment);
        public Task DeleteAppointment(int id);

        Task CancelAppointment(int id);


        public Task<DtoAppointment> ConvertToDto(Appointment appointment);

    }
}
