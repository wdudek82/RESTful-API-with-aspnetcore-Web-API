using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPIApp.Models.Dtos;
using ParkyAPIApp.Repository.IRepository;

namespace ParkyAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            var trails = _trailRepo.GetTrails();
            var trailsDto = trails
                .Select(trail => _mapper.Map<TrailDto>(trail))
                .ToList();
            return Ok(trailsDto);
        }

        [HttpGet("id:int")]
        public IActionResult GetTrail(int id)
        {
            var trail = _trailRepo.GetTrail(id);

            if (trail == null)
            {
                return NotFound();
            }

            return Ok(trail);
        }

        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("id:int")]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto, int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("id:int")]
        public IActionResult DeleteTrail(int id)
        {
            throw new NotImplementedException();
        }
    }
}
