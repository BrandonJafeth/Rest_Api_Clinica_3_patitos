using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentTypes;
using System.Data;
using static Services.Extensions.DtoMapping;

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
        public async Task<IEnumerable<DtoAppointmentType>> Get()
        {
            return await _svAppointmentType.GetAllAppointmentType();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DtoAppointmentType>> Get(int id)
        {
            var appointmentType = await _svAppointmentType.GetAppointmentTypeById(id);

            if (appointmentType == null)
            {
                return NotFound();
            }

            return appointmentType;
        }


    }
}
