using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Clinic_Branches
{
    public class SvClinic_Branch: ISvClinic_Branch
    {
        private readonly MyContext _myDbContext;

        public SvClinic_Branch(MyContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public async Task<List<Clinic_Branch>> GetAllClinic_Branch()
        {
            return await _myDbContext.Clinic_Branches.ToListAsync();
        }

        public async Task<Clinic_Branch> GetClinic_BranchById(int id)
        {
            return await _myDbContext.Clinic_Branches.FindAsync(id);
        }
    }
}
