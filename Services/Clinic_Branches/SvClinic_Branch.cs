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

        public async Task<List<Clinic_Branch>> AddClinic_Brach(List<Clinic_Branch> clinic_branch)
        {

            foreach (var branch in clinic_branch)
            {
                await _myDbContext.Clinic_Branches.AddAsync(branch);
            }

            await _myDbContext.SaveChangesAsync();

            return clinic_branch;
        }

        public async Task DeleteClinic_Brach(int id)
        {
            var branchToDelete = await _myDbContext.Clinic_Branches.FindAsync(id);

            if (branchToDelete == null)
            {
                throw new InvalidOperationException("Branch not found.");
            }

            _myDbContext.Clinic_Branches.Remove(branchToDelete);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<List<Clinic_Branch>> GetAllClinic_Branch()
        {
            return await _myDbContext.Clinic_Branches.ToListAsync();
        }

        public async Task<Clinic_Branch> GetClinic_BranchById(int id)
        {
            return await _myDbContext.Clinic_Branches.FindAsync(id);
        }

        public async Task<Clinic_Branch> UpdateClinic_Branch(int id, Clinic_Branch clinic_branch)
        {
            var Clinic_BrachToUpdate = await _myDbContext.Clinic_Branches.FindAsync(id);

            if (Clinic_BrachToUpdate == null)
            {
                throw new InvalidOperationException("Branch not found.");
            }

            Clinic_BrachToUpdate.Branch_Name = clinic_branch.Branch_Name;

            await _myDbContext.SaveChangesAsync();
            return Clinic_BrachToUpdate;
        }
    }
}
