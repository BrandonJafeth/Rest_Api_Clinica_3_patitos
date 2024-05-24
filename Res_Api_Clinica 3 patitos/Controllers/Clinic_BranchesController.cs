using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Clinic_Branches;

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
        public async Task<IEnumerable<Clinic_Branch>> Get()
        {
            return await _svClinic_Branch.GetAllClinic_Branch();
        }

        [HttpGet("{id}")]
        public async Task<Clinic_Branch> Get(int id)
        {
            return await _svClinic_Branch.GetClinic_BranchById(id);
        }
    }
}
