using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }

        public IActionResult Index()
        {
            return View(new Trail());
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var npList = await _npRepo.GetAllAsync(StaticDetails.NationalParksApiPath);
            var trailVm = new TrailsVm
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // true for Insert/Create
                return View(trailVm);
            }

            trailVm.Trail = await _trailRepo.GetAsync(StaticDetails.TrailsApiPath, id.GetValueOrDefault());
            if (trailVm.Trail == null)
            {
                return NotFound();
            }

            return View(trailVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVm trailsVm)
        {
            if (ModelState.IsValid)
            {
                return View(trailsVm);
            }

            if (trailsVm.Trail.Id == 0)
            {
                await _trailRepo.CreateAsync(StaticDetails.TrailsApiPath, trailsVm.Trail);
            }
            else
            {
                await _trailRepo.UpdateAsync(StaticDetails.TrailsApiPath, trailsVm.Trail);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new
                {
                    data = await _trailRepo.GetAllAsync(StaticDetails.TrailsApiPath)
                }
            );
        }

        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(StaticDetails.TrailsApiPath, id);
            var result = status ? "Successful" : "Not Successful";
            return Json(new {success = false, message = $"Delete {result}"});
        }
    }
}
