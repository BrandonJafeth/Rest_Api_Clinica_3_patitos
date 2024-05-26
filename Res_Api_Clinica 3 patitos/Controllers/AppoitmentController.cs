using Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Appointments;
using static Services.Extensions.DtoMapping;

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
        public async Task<IEnumerable<DtoAppointment>> Get()
        {
            return await _svAppointment.GetAllAppointments();
        }

        [HttpGet("{id}")]
        public async Task<DtoAppointment> Get(int id)
        {
            return await _svAppointment.GetAppointmentById(id);
        }


        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<DtoAppointment>> GetAppointmentsByUserId(int userId)
        {
            return await _svAppointment.GetAppointmentsByUserId(userId);
        }

        [HttpGet("today")]
        public async Task<IEnumerable<DtoAppointment>> GetAppointmentsForToday()
        {
            return await _svAppointment.GetAppointmentsForToday();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DtoAddAppointment dtoAppointment)
        {
            await _svAppointment.AddAppointments(new List<DtoAddAppointment> { dtoAppointment }, "USER");
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DtoAddAppointment dtoAppointment)
        {
            await _svAppointment.UpdateAppointment(id, dtoAppointment, "USER");
            return Ok();
        }

        [HttpPatch("cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _svAppointment.CancelAppointment(id, "USER");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svAppointment.DeleteAppointment(id, "admin");
            return Ok();
        }






    }
}

