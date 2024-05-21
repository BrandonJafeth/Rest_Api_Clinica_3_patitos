using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public class DtoMapping
    {
        public class DtoLogin
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class DtoRegister
        {
            public string User_Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }
    }
}
