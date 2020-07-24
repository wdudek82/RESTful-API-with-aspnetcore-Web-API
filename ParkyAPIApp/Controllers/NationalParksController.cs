using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPIApp.Models;
using ParkyAPIApp.Models.Dtos;
using ParkyAPIApp.Repository.IRepository;

namespace ParkyAPIApp.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepo.GetNationalParks();
            var nationalParksDto = nationalParks
                .Select(nationalPark => _mapper.Map<NationalParkDto>(nationalPark))
                .ToList();

            return Ok(nationalParksDto);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="id">The Id of the National Park</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
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

            return CreatedAtRoute("GetNationalPark",
                new
                {
                    version = HttpContext.GetRequestedApiVersion()!.ToString(),
                    id = nationalPark.Id
                },
                nationalPark);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("message",
                    $"Something went wrong when updating the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }

            var nationalPark = _npRepo.GetNationalPark(id);
            if (!_npRepo.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
