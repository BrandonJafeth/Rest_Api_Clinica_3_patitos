using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Appointments;
using System.Data;
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

        public AppoitmentController(ISvAppointment svAppointment, IAuthorizationService @object) : this(svAppointment)
        {
        }



        // READ
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
        [Authorize(Roles = "USER")]
        public async Task<IEnumerable<DtoAppointment>> GetAppointmentsByUserId(int userId)
        {
            return await _svAppointment.GetAppointmentsByUserId(userId);
        }

        [HttpGet("today")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IEnumerable<DtoAppointment>> GetAppointmentsForToday()
        {
            return await _svAppointment.GetAppointmentsForToday();
        }

        [HttpGet("dates")]
        public async Task<IEnumerable<DateTime>> GetAppointmentDates()
        {
            return await _svAppointment.GetAppointmentDates();
        }

        

        // WRITE
        [HttpPost]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> Post([FromBody] DtoAddAppointment dtoAppointment)
        {
            await _svAppointment.AddAppointments(new List<DtoAddAppointment> { dtoAppointment });
            return Ok();
        }
       
        
        
        
        [HttpPatch("{id}")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult<DtoUpdateAppointment>> Patch(int id, [FromBody] DtoUpdateAppointment dtoAppointment)
        {
            var updatedAppointment = await _svAppointment.UpdateAppointment(id, dtoAppointment);
            return Ok(updatedAppointment);
        }


        [HttpPatch("cancel/{id}")]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _svAppointment.CancelAppointment(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svAppointment.DeleteAppointment(id);
            return Ok();
        }






    }
}

