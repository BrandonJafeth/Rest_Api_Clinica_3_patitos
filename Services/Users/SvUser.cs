using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Extensions;
using Services.MyDbContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Users
{
    public class SvUser : ISvUser
    {
        private readonly MyContext _context;
        private readonly IConfiguration _configuration;

        public SvUser(MyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private  string CreateToken(User user)
        {
            var roleName =  _context.Users.Where(u => u.Id_User == user.Id_User).Select(u => u.Rol.Name_Rol).FirstOrDefault();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.User_Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                new Claim(ClaimTypes.Role, roleName)
        

             };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<List<User>> Get()
        {
            List<User> list = await _context.Users
                .Include(x => x.Rol)
                .ToListAsync();

            return list;
        }

        public async Task<User> GetById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id_User == id);

            return user;
        }

        public async Task<object> Login(DtoMapping.DtoLogin request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return null;
            }

            if (user.Email != request.Email)
            {
                return null;
            }

            if (user.Password != request.Password)
            {
                return null;
            }

            string token = CreateToken(user);


            return token;
        }

        public async Task<User> Register(DtoMapping.DtoRegister request)
        {
            var isExistsPatient = await _context.Users.FirstOrDefaultAsync(p => p.Email == request.Email);

            if (isExistsPatient != null)
            {
                throw new Exception("The user already exists!");
            }

            User newPacient = new User();

            newPacient.User_Name = request.User_Name;
            newPacient.Email = request.Email;
            newPacient.Password = request.Password;
            newPacient.Phone = request.Phone;
            newPacient.Id_Rol = 1;


            _context.Users.Add(newPacient);
            _context.SaveChanges();

            return newPacient;
        }
    }
}
