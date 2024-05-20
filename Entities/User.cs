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
        public string Email { get; set; }
        public string Password { get; set; }


        //Relations
        public Patient Patient { get; set; }
        public string Pat_Name => Patient?.Pat_Name;
        public ICollection<Rol> Roles { get; set; }


    }
}
