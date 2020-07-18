using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPIApp.Data;
using ParkyAPIApp.Models;
using ParkyAPIApp.Models.Dtos;
using ParkyAPIApp.Repository.IRepository;

namespace ParkyAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(ApplicationDbContext db, INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepo.GetNationalParks();
            var nationalParksDto = nationalParks
                .Select(nationalPark => _mapper.Map<NationalParkDto>(nationalPark))
                .ToList();

            return Ok(nationalParksDto);
        }

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var nationalPark = _npRepo.GetNationalPark(id);

            if (nationalPark == null)
            {
                return NotFound();
            }

            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("Message", "National Park Exists!");
                return NotFound(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            var created = _npRepo.CreateNationalPark(nationalPark);

            if (!created)
            {
                ModelState.AddModelError("Message", $"Something went wrong when saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new {id = nationalPark.Id}, nationalPark);
        }
    }
}
