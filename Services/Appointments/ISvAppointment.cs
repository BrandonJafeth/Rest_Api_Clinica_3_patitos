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


        public Task<List<DtoAppointment>> GetAppointmentsByUserName(string User_name);


        //WRITES
        public Task<List<DtoAddAppointment>> AddAppointments(List<DtoAddAppointment> appointments, string role);
        public Task<DtoAddAppointment> UpdateAppointment(int id, DtoAddAppointment appointment, string role);
        public Task DeleteAppointment(int id, string role);

        Task CancelAppointment(int id, string role);


        public Task<DtoAppointment> ConvertToDto(Appointment appointment);

    }
}
