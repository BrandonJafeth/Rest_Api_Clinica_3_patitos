using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User
    {

        public int Id_User { get; set; }
        public string User_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }


        //Relations
    
        public ICollection<Appointment>? Appointments { get; set; }
        public int? Id_Rol { get; set; }
        public Rol? Rol { get; set; }
    }
}
