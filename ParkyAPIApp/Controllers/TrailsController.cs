using System;
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
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var trails = _trailRepo.GetTrails();
            var trailsDto = trails
                .Select(trail => _mapper.Map<TrailDto>(trail))
                .ToList();
            return Ok(trailsDto);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="id">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(Trail))]
        [ProducesResponseType(404)]
        public IActionResult GetTrail(int id)
        {
            var trail = _trailRepo.GetTrail(id);

            if (trail == null)
            {
                return NotFound();
            }

            var trailDto = _mapper.Map<TrailDto>(trail);

            return Ok(trailDto);
        }

        /// <summary>
        /// Get national park trails
        /// </summary>
        /// <param name="nationalParkId">The Id of the national park</param>
        /// <returns></returns>
        [HttpGet("nationalPark/{nationalParkId:int}", Name = "GetTrailsInNationalPark")]
        [ProducesResponseType(200, Type = typeof(Trail))]
        [ProducesResponseType(404)]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var trails = _trailRepo.GetTrailsInNationalPark(nationalParkId);

            if (trails == null)
            {
                return NotFound();
            }

            var trailsDto = trails
                .Select(trail => _mapper.Map<TrailDto>(trail))
                .ToList();

            return Ok(trailsDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailCreateDto)
        {
            if (trailCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepo.TrailExists(trailCreateDto.Name))
            {
                ModelState.AddModelError("Message", "Trail Exists!");
                return NotFound(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailCreateDto);
            var created = _trailRepo.CreateTrail(trail);

            if (!created)
            {
                ModelState.AddModelError("Message", $"Something went wrong when saving trail {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new {id = trail.Id}, trail);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailUpdateDto)
        {
            Console.WriteLine($"id: {id}, trailUpdateDto.Id: {trailUpdateDto.Id}");
            if (id != trailUpdateDto.Id && id > 0)
            {
                return BadRequest(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailUpdateDto);
            if (!_trailRepo.UpdateTrail(trail))
            {
                ModelState.AddModelError("Message", $"Something when wrong when updating the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailRepo.TrailExists(id))
            {
                return NotFound();
            }

            var trail = _trailRepo.GetTrail(id);
            if (!_trailRepo.DeleteTrail(trail))
            {
                ModelState.AddModelError("Message", $"Something went wrong when deleting trail {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
