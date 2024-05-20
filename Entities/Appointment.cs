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

        public int Id_ClinicBranch { get; set; }
        public Clinic_Branch Clinic_Branch { get; set; }

        public int Id_Appoitment_Type { get; set; }
        public AppointmentType AppointmentType { get; set; }

        public int Id_User { get; set; }

        public User User { get; set; }


    }
}
