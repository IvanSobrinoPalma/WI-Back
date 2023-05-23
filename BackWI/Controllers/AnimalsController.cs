using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackWI.Models;
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
            List<Animal> animals = new List<Animal>();

            try
            {
                animals = _context.Animals.Include(o => o.TypeAnimal).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", reponse = animals });
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, reponse = animals });
            }
        }

        [HttpGet]
        [Route("getAnimal/{NameAnimal}")]
        public IActionResult ListAnimals(string NameAnimal)
        {
            Animal animal = _context.Animals.Find(NameAnimal);

            if(animal == null) 
            {
                return BadRequest("Animal no encontrado");
            }

            try
            {
                animal = _context.Animals.Include(o => o.TypeAnimal).Where(a => a.NameAnimal == NameAnimal).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "ok", reponse = animal });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message, reponse = animal });
            }


        }

        [HttpPost]
        [Route("saveAnimal")]
        public IActionResult SaveAnimal([FromBody] Animal animal) 
        {
            try
            {
                _context.Animals.Add(animal);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "ok"});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message});
            }
        }

        [HttpDelete]
        [Route("deleteAnimal/{IdAnimal: string}")]
        public IActionResult DeleteAnimal(string IdAnimal)
        {
            Animal _animal = _context.Animals.Find(IdAnimal);

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
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }

        [HttpPut]
        [Route("updateAnimal")]
        public IActionResult UpdateAnimal([FromBody] Animal animal)
        {
            Animal _animal = _context.Animals.Find(animal.IdAnimal);

            if (_animal == null)
            {
                return BadRequest("Animal no encontrado");
            }

            try
            {
                _animal.NameAnimal = animal.NameAnimal is null ? _animal.NameAnimal : animal.NameAnimal;
                _animal.Type = animal.Type is null ? _animal.Type : animal.Type;
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
                return StatusCode(StatusCodes.Status200OK, new { message = ex.Message });
            }
        }
    }

}
