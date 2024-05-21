using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Appointments;

namespace API_PruebaEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoitmentController : ControllerBase
    {
        private readonly ISvAppointment _svAppointment;

        public AppoitmentController(ISvAppointment svAppointment)
        {
            _svAppointment = svAppointment;
        }

        [HttpGet]
        public async Task<IEnumerable<Appointment>> Get()
        {
            return await _svAppointment.GetAllAppointments();
        }

        [HttpGet("{id}")]
        public async Task<Appointment> Get(int id)
        {
            return await _svAppointment.GetAppointmentById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Appointment appointment)
        {
            await _svAppointment.AddAppointments(new List<Appointment> { appointment },"USER");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Appointment appointment)
        {
            await _svAppointment.UpdateAppointment(id, appointment,"USER");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svAppointment.DeleteAppointment(id, "admin");
            return Ok();
        }
    }
}
