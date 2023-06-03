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
        public readonly ITokenService _token;

        public UsersController(WildInfoContext context, IJwtProvider jwd, ITokenService token)
        {
            _context = context;
            _jwtProvider = jwd;
            _token = token;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<IActionResult> ListUsers()
        {
            List<Users> user = new List<Users>();

            try
            {
                user = await _context.Users.ToListAsync();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
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

        [Authorize(Policy = "owner")]
        [HttpPost]
        [Route("addAdmin")]
        public async Task<IActionResult> SaveAdminUser(Users user)
        {
            try
            {
                user.Passwordd = BCrypt.Net.BCrypt.HashPassword(user.Passwordd);
                user.Roll = "admin";
                await _context.Users.AddAsync(user);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPost]
        [Route("addClient")]
        public async Task<IActionResult> SaveClientUser(Users user)
        {
            try
            {
                user.Passwordd = BCrypt.Net.BCrypt.HashPassword(user.Passwordd);
                user.Roll = "client";
                await _context.Users.AddAsync(user);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [Authorize(Policy = "owner")]
        [HttpDelete]
        [Route("deleteUser/{IdUser}")]
        public async Task<IActionResult> DeleteUser(Guid IdUser)
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
            Guid idUser = _token.GetContentByToken(HttpContext, "idUser");

            if (_user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            } else if(idUser != user.IdUser)
            {
                return Unauthorized(new { message = "No son tus datos" });
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

