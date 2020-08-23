using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<T> GetAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{url}{id}");
            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
            {
                IncludeAuthToken(client, token);
            }

            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            var jsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
            {
                IncludeAuthToken(client, token);
            }

            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public async Task<bool> CreateAsync(string url, T objToCreate, string token = "")
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
            if (token.Length != 0)
            {
                IncludeAuthToken(client, token);
            }

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);

            if (objToUpdate == null) return false;

            request.Content = new StringContent(
                JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
            {
                IncludeAuthToken(client, token);
            }

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync(string url, int id, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{url}{id}");

            var client = _clientFactory.CreateClient();
            if (token.Length != 0)
            {
                IncludeAuthToken(client, token);
            }

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        private static void IncludeAuthToken(HttpClient client, string token = "")
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
