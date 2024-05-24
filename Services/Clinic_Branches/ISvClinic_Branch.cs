using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Clinic_Branches
{
    public interface ISvClinic_Branch
    {
        public Task<List<Clinic_Branch>> GetAllClinic_Branch();
        public Task<Clinic_Branch> GetClinic_BranchById(int id);
    }
}
