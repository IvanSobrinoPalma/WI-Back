using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackWI.Model;
using Microsoft.AspNetCore.Cors;
namespace BackWI.Controllers
{
    [EnableCors("reglasPD")]
    [Route("api/[controller]")]
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
        public IActionResult ListAnimals()
        {
            List<Animals> animals = new List<Animals>();

            try
            {
                animals = _context.Animals.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", reponse = animals });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = animals });
            }
        }

        [HttpGet]
        [Route("getAnimal/{IdAnimal}")]
        public IActionResult ListAnimals(Guid IdAnimal)
        {
            Animals animal = _context.Animals.Find(IdAnimal);

            if (animal == null)
            {
                return BadRequest("Animal no encontrado");
            }

            try
            {
                animal = _context.Animals.Include(o => o.TypeAnimalNavigation).FirstOrDefault(a => a.IdAnimal == IdAnimal);

                return StatusCode(StatusCodes.Status200OK, new { message = "ok", reponse = animal });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = animal });
            }


        }

        [HttpPost]
        [Route("saveAnimal")]
        public async Task<IActionResult> SaveAnimal(Animals animal) 
        {
            try
            {
                await _context.Animals.AddAsync(animal);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message});
            }
        }

        [HttpDelete]
        [Route("deleteAnimal/{IdAnimal}")]
        public IActionResult DeleteAnimal(Guid IdAnimal)
        {
            Animals _animal = _context.Animals.Find(IdAnimal);

            if (_animal == null)
            {
                return BadRequest("Animal no encontrado");
            }

            try
            {
                _context.Animals.Remove(_animal);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("updateAnimal")]
        public IActionResult UpdateAnimal([FromBody] Animals animal)
        {
            Animals _animal = _context.Animals.Find(animal.IdAnimal);

            if (_animal == null)
            {
                return BadRequest("Animal no encontrado");
            }

            try
            {
                _animal.NameAnimal = animal.NameAnimal is null ? _animal.NameAnimal : animal.NameAnimal;
                _animal.TypeAnimal = animal.TypeAnimal;
                _animal.ScientificName = animal.ScientificName is null ? _animal.ScientificName : animal.ScientificName;
                _animal.Image = animal.Image is null ? _animal.Image : animal.Image;
                _animal.DangerOfExtinction = animal.DangerOfExtinction;
                _animal.Dangerousness = animal.Dangerousness;

                _context.Animals.Update(_animal);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }
    }

}
