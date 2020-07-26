using System.Net.Http;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
