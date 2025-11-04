namespace Backend.DataSource.Interfaces
{
    public interface IDataSource<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
    }
}
