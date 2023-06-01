using BackWI.Data;
using BackWI.Models;
using BackWI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace BackWI.Controllers
{
    [EnableCors("reglasPD")]
    [Route("wi/picture")]
    [ApiController]
    public class PhotographsController : ControllerBase
    {
        public readonly WildInfoContext _context;
        public readonly ITokenService _token;

        public PhotographsController(WildInfoContext context, ITokenService token)
        {
            _context = context;
            _token = token;
        }

        [HttpGet]
        [Route("getPictures/{IdUser}")]
        public async Task<IActionResult> ListPictures(Guid IdUser)
        {
            List<Photographs> user = new List<Photographs>();

            Guid idUser = _token.GetContentByToken(HttpContext, "idUser");

            if (idUser != IdUser)
            {
                return Unauthorized(new { message = "No son tus datos" });
            }

            try
            {
                user = await _context.Photographs.Where(p => p.IdUser == IdUser).ToListAsync();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPost]
        [Route("getPicture")]
        public async Task<IActionResult> GetPictures(Guid IdUser, Guid IdAnimal)
        {
            Photographs user = new Photographs();
            Guid idUser = _token.GetContentByToken(HttpContext, "idUser");

            if (idUser != IdUser)
            {
                return Unauthorized(new { message = "No son tus datos" });
            }

            try
            {
                user = await _context.Photographs.Where(p => p.IdUser == IdUser).Where(p => p.IdAnimal == IdAnimal).FirstOrDefaultAsync();

                return Ok(new { message = "ok", response = user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPost]
        [Route("savePicture")]
        public async Task<IActionResult> SavePicture(Photographs picture)
        {
            try
            {
                await _context.Photographs.AddAsync(picture);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = picture });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpDelete]
        [Route("deletePicture")]
        public async Task<IActionResult> DeletePicture(Guid IdUser, Guid IdAnimal)
        {
            Photographs _picture = await _context.Photographs.Where(p => p.IdUser == IdUser).Where(p => p.IdAnimal == IdAnimal).FirstOrDefaultAsync();

            if (_picture == null)
            {
                return BadRequest(new { message = "Foto no encontrada" });
            }
            try
            {
                _context.Photographs.Remove(_picture);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = _picture });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPut]
        [Route("updatePicture")]
        public async Task<IActionResult> UpdatePicture(Photographs picture)
        {
            Photographs _picture = await _context.Photographs.Where(p => p.IdUser == picture.IdUser).Where(p => p.IdAnimal == picture.IdAnimal).FirstOrDefaultAsync();
            Guid idUser = _token.GetContentByToken(HttpContext, "idUser");

            if (_picture == null)
            {
                return BadRequest(new { message = "Usuario no encontrado" });
            }
            else if (idUser != picture.IdUser)
            {
                return Unauthorized(new { message = "No son tus datos" });
            }

            try
            {
                _picture.Image = picture.Image ?? _picture.Image;
          
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
