﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Rol
    {
        public int Id_Rol { get; set; }
        public string Name_Rol { get; set; }

        //Relations

        public ICollection<User> Users { get; set; }
      

    }
}
