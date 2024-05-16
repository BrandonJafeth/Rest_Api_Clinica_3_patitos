using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public User User { get; set; }
        public AppointmentType AppointmentType { get; set; }
    }
}
