using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AppointmentType
    {
        public int AppointmentTypeId { get; set; }
        public string Name { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
