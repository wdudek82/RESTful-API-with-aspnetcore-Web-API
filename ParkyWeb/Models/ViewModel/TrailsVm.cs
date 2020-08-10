using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Models.ViewModel
{
    public class TrailsVm
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
