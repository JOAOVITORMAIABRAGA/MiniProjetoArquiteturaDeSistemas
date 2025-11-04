using backend.DataSources;
using backend.Models;
using Backend.Models;

namespace Backend.Models.DiscenteServices
{
    public class DiscenteService
    {
        private readonly DiscenteAwsDataSource _awsSource;
        private readonly DiscenteDbDataSource _dbSource;

        public DiscenteService(DiscenteAwsDataSource awsSource, DiscenteDbDataSource dbSource)
        {
            _awsSource = awsSource;
            _dbSource = dbSource;
        }

        // usado no DataSeeder → pega da AWS e escreve no DB
        public async Task<List<Discente>> ImportarDiscentesDaAws()
        {
            return await _awsSource.ImportarParaBancoAsync(_dbSource);
        }

        // usado pelos Controllers → pega DO BANCO
        public async Task<List<Discente>> GetDiscentes()
        {
            return await _dbSource.GetAllAsync();
        }

        public async Task<Discente?> GetDiscenteById(int id)
        {
            return await _dbSource.GetByIdAsync(id);
        }
    }
}
