using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackWI.Models;
using Microsoft.AspNetCore.Cors;
using BackWI.Data;
using Microsoft.AspNetCore.Authorization;
using BackWI.Services;

namespace BackWI.Controllers
{
    [EnableCors("reglasPD")]
    [Route("wi/animal")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        public readonly WildInfoContext _context;

        public AnimalsController(WildInfoContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAnimals")]
        public async Task<IActionResult> ListAnimals()
        {
            List<Animals> animals = new List<Animals>();

            try
            {
                animals = await _context.Animals.ToListAsync();
                return Ok(new { message = "ok", response = animals });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpGet]
        [Route("getAnimal/{IdAnimal}")]
        public async Task<IActionResult> ListAnimals(Guid IdAnimal)
        {
            Animals animal = await _context.Animals.FindAsync(IdAnimal);

            if (animal == null)
            {
                return BadRequest(new { message = "Animal no encontrado" });
            }

            try
            {
                animal = await _context.Animals.Include(o => o.TypeAnimalNavigation).FirstOrDefaultAsync(a => a.IdAnimal == IdAnimal);

                return Ok(new { message = "ok", response = animal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }


        }

        [Authorize(Policy = "owner")]
        [HttpPost]
        [Route("addAnimal")]
        public async Task<IActionResult> SaveAnimal(Animals animal) 
        {
            try
            {
                await _context.Animals.AddAsync(animal);
                _context.SaveChanges();

                return Ok(new { message = "ok", response = animal});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [Authorize(Policy = "owner")]
        [HttpDelete]
        [Route("deleteAnimal/{IdAnimal}")]
        public async Task<IActionResult> DeleteAnimal(Guid IdAnimal)
        {
            Animals _animal = await _context.Animals.FindAsync(IdAnimal);

            if (_animal == null)
            {
                return BadRequest(new { message = "Animal no encontrado" });
            }

            try
            {
                _context.Animals.Remove(_animal);
                _context.SaveChanges();

                return Ok(new { message = "ok" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message, trace = ex.StackTrace });
            }
        }

        [HttpPut]
        [Route("updateAnimal")]
        public async Task<IActionResult> UpdateAnimal(Animals animal)
        {
            Animals _animal = await _context.Animals.FindAsync(animal.IdAnimal);

            if (_animal == null)
            {
                return BadRequest(new { message = "Animal no encontrado" });
            }

            try
            {
                _animal.NameAnimal = animal.NameAnimal ?? _animal.NameAnimal;
                _animal.TypeAnimal = animal.TypeAnimal;
                _animal.ScientificName = animal.ScientificName ?? _animal.ScientificName;
                _animal.Image = animal.Image ?? _animal.Image;
                _animal.DangerOfExtinction = animal.DangerOfExtinction;
                _animal.Dangerousness = animal.Dangerousness;
                _animal.TypeAnimalNavigation = animal.TypeAnimalNavigation ?? _animal.TypeAnimalNavigation;

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
