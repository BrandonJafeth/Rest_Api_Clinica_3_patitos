using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Clinic_Branches;
using static Services.Extensions.DtoMapping;

namespace Res_Api_Clinica_3_patitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Clinic_BranchesController : Controller
    {
        private readonly ISvClinic_Branch _svClinic_Branch;

        public Clinic_BranchesController(ISvClinic_Branch svClinic_Branch)
        {
            _svClinic_Branch = svClinic_Branch;
        }


        [HttpGet()]
        public async Task<IEnumerable<DtoClinicBranch>> Get()
        {
            return await _svClinic_Branch.GetAllClinic_Branch();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoClinicBranch>> Get(int id)
        {
            var clinicBranch = await _svClinic_Branch.GetClinic_BranchById(id);

            if (clinicBranch == null)
            {
                return NotFound();
            }

            return clinicBranch;
        }

    }
}
