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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AppointmentType appointmentType)
        {
            await _svAppointmentType.AddAppointmentType(new List<AppointmentType> { appointmentType });
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AppointmentType appointmentType)
        {
            await _svAppointmentType.UpdateAppointmentType(id, appointmentType);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svAppointmentType.DeleteAppointmentType(id);
            return Ok();
        }
    }
}
