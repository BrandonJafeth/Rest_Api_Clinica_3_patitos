using Entities;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Appointments
{
    public interface ISvAppointment
    {
        //READS 
        public Task<List<Appointment>> GetAllAppointments();
        public Task<Appointment> GetAppointmentById(int id);

        //WRITES
        public Task<List<Appointment>> AddAppointments(List<Appointment> appointments);
        public Task<Appointment> UpdateAppointment(int id, Appointment appointment);
        public Task DeleteAppointment(int id, string role);
    }
}
