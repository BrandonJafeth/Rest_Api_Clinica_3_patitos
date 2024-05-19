using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {

        public int Id_User{ get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        //Relations
        public List<Appointment> Appointments { get; set; }

    }
}
