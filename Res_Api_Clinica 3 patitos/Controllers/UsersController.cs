using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Users;
using static Services.Extensions.DtoMapping;

namespace Res_Api_Clinica_3_patitos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISvUser _service;

        public UsersController(ISvUser service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<List<User>> Get()
        {
            return _service.Get();
        }

        [HttpPost("login")]
        public async Task<object> Login(DtoLogin request)
        {
            var user = await _service.Login(request);

            if(user == null)
            {
                return BadRequest("User not found!");
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(DtoRegister request)
        {
            User user = await _service.Register(request);

            return Ok(user);
        }
    }
}
