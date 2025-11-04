using backend.DataSources;
using backend.Models;

namespace Backend.Models.BibliotecaServices
{
    public class BibliotecaService
    {
        private readonly LivroAwsDataSource _awsSource;
        private readonly LivroDbDataSource _dbSource;

        public BibliotecaService(LivroAwsDataSource awsSource, LivroDbDataSource dbSource)
        {
            _awsSource = awsSource;
            _dbSource = dbSource;
        }

        // usado no DataSeeder → pega da AWS e escreve no DB
        public async Task<List<Livro>> ImportarLivrosDaAws()
        {
            return await _awsSource.ImportarParaBancoAsync(_dbSource);
        }

        // usado pelos Controllers → pega DO BANCO
        public async Task<List<Livro>> GetLivros()
        {
            return await _dbSource.GetAllAsync();
        }

        public async Task<Livro?> GetLivroById(int id)
        {
            return await _dbSource.GetByIdAsync(id);
        }
    }
}