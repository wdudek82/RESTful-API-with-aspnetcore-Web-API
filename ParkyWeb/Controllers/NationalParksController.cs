using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    [Authorize]
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
                data = await _npRepo.GetAllAsync(StaticDetails.NationalParksApiPath, GetToken())
            });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalPark = new NationalPark();
            if (id == null)
            {
                return View(nationalPark);
            }

            nationalPark =
                await _npRepo.GetAsync(StaticDetails.NationalParksApiPath, id.GetValueOrDefault(), GetToken());
            if (nationalPark == null)
            {
                return NotFound();
            }

            return View(nationalPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (!ModelState.IsValid) return View(nationalPark);

            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null;
                using (var fs1 = files[0].OpenReadStream())
                {
                    using (var ms1 = new MemoryStream())
                    {
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                }

                nationalPark.Picture = p1;
            }
            else
            {
                var objFromDb = await _npRepo.GetAsync(StaticDetails.NationalParksApiPath, nationalPark.Id, GetToken());
                nationalPark.Picture = objFromDb.Picture;
            }

            if (nationalPark.Id == 0)
            {
                await _npRepo.CreateAsync(StaticDetails.NationalParksApiPath, nationalPark, GetToken());
            }
            else
            {
                await _npRepo.UpdateAsync(StaticDetails.NationalParksApiPath + nationalPark.Id, nationalPark,
                    GetToken());
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(StaticDetails.NationalParksApiPath, id, GetToken());
            var infix = status ? "" : "Not";
            return Json(new {success = status, message = $"Delete {infix} Successful"});
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("JWToken") ?? "";
        }
    }
}
