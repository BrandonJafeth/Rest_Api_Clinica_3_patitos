using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentTypes;
using System.Data;

namespace Res_Api_Clinica_3_patitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoitmentTypeController : Controller
    {
        private readonly ISvAppointmentType _svAppointmentType;

        public AppoitmentTypeController(ISvAppointmentType svAppointmentType)
        {
            _svAppointmentType = svAppointmentType;
        }

      
        [HttpGet()]
        public async Task<IEnumerable<AppointmentType>> Get()
        {
            return await _svAppointmentType.GetAllAppointmentType();
        }
        [HttpGet("{id}")]
        public async Task<AppointmentType> Get(int id)
        {
            return await _svAppointmentType.GetAppointmentTypeById(id);
        }

    }
}
