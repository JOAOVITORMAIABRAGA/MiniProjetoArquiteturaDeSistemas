using backend.DataSources;
using backend.Models;
using Backend.Models;

namespace Backend.Models.DisciplinaServices
{
    public class DisciplinaService
    {
        private readonly DisciplinaAwsDataSource _awsSource;
        private readonly DisciplinaDbDataSource _dbSource;

        public DisciplinaService(DisciplinaAwsDataSource awsSource, DisciplinaDbDataSource dbSource)
        {
            _awsSource = awsSource;
            _dbSource = dbSource;
        }

        // usado no DataSeeder → pega da AWS e escreve no DB
        public async Task<List<Disciplina>> ImportarDisciplinasDaAws()
        {
            return await _awsSource.ImportarParaBancoAsync(_dbSource);
        }

        // usado pelos Controllers → pega DO BANCO
        public async Task<List<Disciplina>> GetDisciplinas()
        {
            return await _dbSource.GetAllAsync();
        }

        public async Task<Disciplina?> GetDisciplinaById(int id)
        {
            return await _dbSource.GetByIdAsync(id);
        }
    }
}