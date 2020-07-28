using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark());
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new
            {
                data = await _npRepo.GetAllAsync(StaticDetails.NationalParksApiPath)
            });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Console.WriteLine("=== Upsert");

            var nationalPark = new NationalPark();
            if (id == null)
            {
                return View(nationalPark);
            }

            nationalPark = await _npRepo.GetAsync(StaticDetails.NationalParksApiPath, id.GetValueOrDefault());
            Console.WriteLine($"=== NP id: {nationalPark.Id}");
            if (nationalPark == null)
            {
                return NotFound();
            }

            return View(nationalPark);
        }
    }
}
