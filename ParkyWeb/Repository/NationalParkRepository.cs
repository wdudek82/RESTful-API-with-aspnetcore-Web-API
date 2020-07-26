using System.Net.Http;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : Repository<NationalPark>, INationalParkRepository
    {
        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
