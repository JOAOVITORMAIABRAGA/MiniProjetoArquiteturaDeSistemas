using backend.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataSource.Interfaces;

namespace backend.DataSources
{
    public class DisciplinaAwsDataSource : IDataSource<Disciplina>
    {
        private readonly HttpClient _httpClient;
        private const string Url = "https://sswfuybfs8.execute-api.us-east-2.amazonaws.com/disciplinaServico/msDisciplina";

        public DisciplinaAwsDataSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Disciplina>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Disciplina>>(Url);
            return response ?? new List<Disciplina>();
        }

        public async Task<Disciplina?> GetByIdAsync(int id)
        {
            var all = await GetAllAsync();
            return all.Find(d => d.Id == id);
        }

    }
}