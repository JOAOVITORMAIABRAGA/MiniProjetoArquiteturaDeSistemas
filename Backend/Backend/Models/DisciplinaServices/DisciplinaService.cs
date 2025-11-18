using backend.DataSources;
using backend.Models;
using System.Text.Json;

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

        public async Task<List<Disciplina>> ImportarDisciplinasDaAws()
        {
            try
            {
                var disciplinas = await _awsSource.GetAllAsync();

                if (disciplinas == null || disciplinas.Count == 0)
                {
                    // Em vez de lançar uma exceção, retorne uma lista vazia.
                    return new List<Disciplina>();
                }

                await _dbSource.SaveRangeAsync(disciplinas);

                return disciplinas;
            }
            catch (JsonException ex)
            {
                // Em vez de lançar uma exceção, logue o erro (opcional) e retorne uma lista vazia.
                Console.WriteLine($"Erro ao desserializar o JSON de disciplinas: {ex.Message}");
                return new List<Disciplina>();
            }
            catch (Exception ex)
            {
                // Em vez de lançar uma exceção geral, logue o erro (opcional) e retorne uma lista vazia.
                Console.WriteLine($"Erro ao importar disciplinas da AWS: {ex.Message}");
                return new List<Disciplina>();
            }
        }

        public async Task<List<Disciplina>> GetDisciplinas() =>
            await _dbSource.GetAllAsync();

        public async Task<Disciplina?> GetDisciplinaById(int id) =>
            await _dbSource.GetByIdAsync(id);
    }
}