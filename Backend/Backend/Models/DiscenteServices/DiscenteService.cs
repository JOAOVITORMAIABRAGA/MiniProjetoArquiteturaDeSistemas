using Backend.Models;
using Backend.Models.DiscenteServices;
using backend.DataSources;
using System.Text.Json;

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
            try
            {
                // 1️⃣ Ler dados da AWS
                var discentes = await _awsSource.GetAllAsync();

                // 2️⃣ Validar JSON vazio ou nulo
                if (discentes == null || discentes.Count == 0)
                {
                    // Em vez de lançar uma exceção, retorne uma lista vazia.
                    Console.WriteLine("Aviso: Nenhum discente foi retornado da AWS. Retornando lista vazia.");
                    return new List<Discente>();
                }

                // 3️⃣ Salvar no DB
                await _dbSource.SaveRangeAsync(discentes);

                return discentes;
            }
            catch (JsonException ex)
            {
                // Em vez de lançar uma exceção, logue o erro e retorne uma lista vazia.
                Console.WriteLine($"Erro ao desserializar o JSON de discentes: {ex.Message}");
                return new List<Discente>();
            }
            catch (Exception ex)
            {
                // Em vez de lançar uma exceção geral, logue o erro e retorne uma lista vazia.
                Console.WriteLine($"Erro ao importar discentes da AWS: {ex.Message}");
                return new List<Discente>();
            }
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