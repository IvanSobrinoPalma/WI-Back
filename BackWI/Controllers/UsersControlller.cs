using BackWI.Data;
using BackWI.Models;
using BackWI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackWI.Controllers
{
    [EnableCors("reglasPD")]
    [Route("wi/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly WildInfoContext _context;
        public readonly IJwtProvider _jwtProvider;

        public UsersController(WildInfoContext context, IJwtProvider jwd)
        {
            _context = context;
            _jwtProvider = jwd;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Users user)
        {
            Users _user = await _context.Users.FirstOrDefaultAsync(u => u.Nick == user.Nick);

            if (_user != null && BCrypt.Net.BCrypt.Verify(user.Passwordd, _user.Passwordd))
            {
                try
                {
                    string token = _jwtProvider.CreateToken(_user);
                    return Ok(new { message = "ok", response = token });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
                }
            }

            return BadRequest(new { message = "No existe ese usuario" });
        }

        [HttpGet]
        [Route("getUser/{IdUser}")]
        public async Task<IActionResult> ListUser(Guid IdUser)
        {
            Users user = await _context.Users.FindAsync(IdUser);

            if (user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            try
            {
                user = await _context.Users.FirstOrDefaultAsync(a => a.IdUser == IdUser);

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPost]
        [Route("saveUser")]
        public async Task<IActionResult> SaveAnimal(Users user)
        {
            try
            {
                user.Passwordd = BCrypt.Net.BCrypt.HashPassword(user.Passwordd);
                await _context.Users.AddAsync(user);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpDelete]
        [Route("deleteUser/{IdUser}")]
        public async Task<IActionResult> DeleteAnimal(Guid IdUser)
        {
            Users _user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == IdUser);

            if (_user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado"});
            }

            try
            {
                _context.Users.Remove(_user);
                _context.SaveChanges();

                return Ok(new { message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPut]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUser(Users user)
        {
            Users _user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == user.IdUser);

            if (_user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }

            try
            {
                _user.NameUser = user.NameUser ?? _user.NameUser;
                _user.Nick = user.Nick ?? _user.Nick;
                _user.FirtsSurname = user.FirtsSurname ?? _user.FirtsSurname;
                _user.SecondSurname = user.SecondSurname ?? _user.SecondSurname;
                _user.Email = user.Email ?? _user.Email;
                _user.Passwordd = BCrypt.Net.BCrypt.HashPassword(user.Passwordd) ?? _user.Passwordd;
                _user.Roll = user.Roll ?? _user.Roll;

                _context.SaveChanges();

                return Ok(new { message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }
    }

}

