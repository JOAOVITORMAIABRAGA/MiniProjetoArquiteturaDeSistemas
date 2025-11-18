using backend.DataSources;
using backend.Models;
using System.Text.Json;

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

        public async Task<List<Livro>> ImportarLivrosDaAws()
        {
            try
            {
                var livros = await _awsSource.GetAllAsync();

                if (livros == null || livros.Count == 0)
                {
                    // Em vez de lançar uma exceção, retorne uma lista vazia.
                    Console.WriteLine("Aviso: Nenhum livro foi retornado da AWS. Retornando lista vazia.");
                    return new List<Livro>();
                }

                await _dbSource.SaveRangeAsync(livros);

                return livros;
            }
            catch (JsonException ex)
            {
                // Em vez de lançar uma exceção, logue o erro e retorne uma lista vazia.
                Console.WriteLine($"Erro ao desserializar o JSON de livros: {ex.Message}");
                return new List<Livro>();
            }
            catch (Exception ex)
            {
                // Em vez de lançar uma exceção geral, logue o erro e retorne uma lista vazia.
                Console.WriteLine($"Erro ao importar livros da AWS: {ex.Message}");
                return new List<Livro>();
            }
        }

        public async Task<List<Livro>> GetLivros() =>
            await _dbSource.GetAllAsync();

        public async Task<Livro?> GetLivroById(int id) =>
            await _dbSource.GetByIdAsync(id);
    }
}