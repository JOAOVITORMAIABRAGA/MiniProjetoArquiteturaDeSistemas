using backend.Models;
using Backend.DataSource.Interfaces;
using Backend.Models;
using System.Net.Http.Json;

namespace backend.DataSources
{
    public class DiscenteAwsDataSource : IDataSource<Discente>
    {
        private readonly HttpClient _httpClient;
        private readonly string _url = "https://rmi6vdpsq8.execute-api.us-east-2.amazonaws.com/msAluno";

        public DiscenteAwsDataSource(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<List<Discente>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Discente>>(_url)
                   ?? new List<Discente>();
        }

        public async Task<Discente?> GetByIdAsync(int id)
        {
            var all = await GetAllAsync();
            return all.Find(d => d.Id == id);
        }

    }
}
