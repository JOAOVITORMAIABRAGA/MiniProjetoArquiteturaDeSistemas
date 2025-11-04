using backend.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.DataSource.Interfaces;

namespace backend.DataSources
{
    public class LivroAwsDataSource : IDataSource<Livro>
    {
        private readonly HttpClient _httpClient;
        private const string Url = "https://qiiw8bgxka.execute-api.us-east-2.amazonaws.com/acervo/biblioteca";

        public LivroAwsDataSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Livro>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Livro>>(Url);
            return response ?? new List<Livro>();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            var all = await GetAllAsync();
            return all.Find(l => l.Id == id);
        }

        // Função extra: importa da AWS e grava no DB (paralelo ao Discente)
        public async Task<List<Livro>> ImportarParaBancoAsync(LivroDbDataSource dbSource)
        {
            var livros = await GetAllAsync();
            await dbSource.SaveRangeAsync(livros);
            return livros;
        }
    }
}