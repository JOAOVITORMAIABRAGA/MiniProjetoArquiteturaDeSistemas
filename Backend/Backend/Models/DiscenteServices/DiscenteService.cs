using Backend.Models;
using Backend.Models.DiscenteServices;
using backend.DataSources;

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

        public async Task<List<Discente>> ImportarDiscentesDaAws()
        {
            return await _awsSource.ImportarParaBancoAsync(_dbSource);
        }

        public async Task<List<Discente>> GetDiscentes() =>
            await _dbSource.GetAllAsync();

        public async Task<Discente?> GetDiscenteById(int id) =>
            await _dbSource.GetByIdAsync(id);

        public async Task<(bool success, string message)> AlterarStatusAsync(int id, string novoStatus)
        {
            var discente = await _dbSource.GetByIdAsync(id);
            if (discente == null)
                return (false, "Discente não encontrado.");

            discente.Status = novoStatus;
            await _dbSource.UpdateAsync(discente);

            return (true, $"Status atualizado para: {novoStatus}");
        }
    }
}
