using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;


namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public Task<T> GetAsync(string url, int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(string url)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{url}{id}");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
