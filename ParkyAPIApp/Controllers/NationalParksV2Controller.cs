using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPIApp.Models.Dtos;
using ParkyAPIApp.Repository.IRepository;

namespace ParkyAPIApp.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
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
            var nationalPark = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(nationalPark));
        }
    }
}
