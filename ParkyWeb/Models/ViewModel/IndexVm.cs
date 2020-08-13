using System.Collections.Generic;

namespace ParkyWeb.Models.ViewModel
{
    public class IndexVm
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }
        public IEnumerable<Trail> Trails { get; set; }
    }
}
