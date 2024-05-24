using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.Extensions.DtoMapping;

namespace Services.Clinic_Branches
{
    public class SvClinic_Branch: ISvClinic_Branch
    {
        private readonly MyContext _myDbContext;

        public SvClinic_Branch(MyContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task<List<DtoClinicBranch>> GetAllClinic_Branch()
        {
            var clinicBranches = await _myDbContext.Clinic_Branches.ToListAsync();

            return clinicBranches.Select(MapToDto).ToList();
        }

        public async Task<DtoClinicBranch> GetClinic_BranchById(int id)
        {
            var clinicBranch = await _myDbContext.Clinic_Branches.FindAsync(id);

            return MapToDto(clinicBranch);
        }

        private DtoClinicBranch MapToDto(Clinic_Branch clinicBranch)
        {
            return new DtoClinicBranch
            {
                Id_ClinicBranch = clinicBranch.Id_ClinicBranch,
                Branch_Name = clinicBranch.Branch_Name
            };
        }

    }
}
