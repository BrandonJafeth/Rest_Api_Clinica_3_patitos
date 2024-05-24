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

        public Task<List<Clinic_Branch>> AddClinic_Brach(List<Clinic_Branch> clinic_branch);
        public Task<Clinic_Branch> UpdateClinic_Brach(int id, Clinic_Branch clinic_branch);
        public Task DeleteClinic_Brach(int id);
        Task<Clinic_Branch> UpdateClinic_Branch(int id, Clinic_Branch clinic_branch);
    }
}
