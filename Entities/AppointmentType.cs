using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AppointmentType
    {
        public int Id_Appoitment_Type { get; set; }
        public string Name_type { get; set; }
     

        //Relations

        public ICollection<Appointment> Appointments { get; set; }


    }
}
