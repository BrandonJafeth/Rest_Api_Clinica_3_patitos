using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Clinic_Branch
    {
        public int Id_ClinicBranch{ get; set; }
        public string Branch_Name { get; set; }

        //Relations
        public int Id_Appoitment { get; set; }
        public Appointment Appointment { get; set; }
    }
}
