using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Appointment
    {
        public int Id_Appoitment { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }

        //Relations


        public ICollection<AppointmentType> AppointmentTypes { get; set; }
        public ICollection<Patient> Patients { get; set; }

        public ICollection<Clinic_Branch> Clinic_Branches { get; set; }

    }
}
