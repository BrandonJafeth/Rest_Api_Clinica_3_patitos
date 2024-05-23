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


        //WRITES
        public Task<List<Appointment>> AddAppointments(List<Appointment> appointments, string role);
        public Task<Appointment> UpdateAppointment(int id, Appointment appointment, string role);
        public Task DeleteAppointment(int id, string role);



        public Task<DtoAppointment> ConvertToDto(Appointment appointment);

    }
}
