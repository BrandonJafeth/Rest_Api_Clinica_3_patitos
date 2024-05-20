
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
        public bool Status { get; set; }
        public DateTime Date { get; set; }
     
    

        //Relations

        public int Id_ClinicBranch { get; set; }
        public Clinic_Branch Clinic_Branch { get; set; }

        public int Id_Appoitment_Type { get; set; }
        public AppointmentType AppointmentType { get; set; }

        public int Id_User { get; set; }

        public User User { get; set; }
    }
}
