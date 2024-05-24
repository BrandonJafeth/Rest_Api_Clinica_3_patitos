using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.Clinic_Branches
{
    public interface ISvClinic_Branch
    {
        public Task<List<DtoClinicBranch>> GetAllClinic_Branch();
        public Task<DtoClinicBranch> GetClinic_BranchById(int id);
    }

}
