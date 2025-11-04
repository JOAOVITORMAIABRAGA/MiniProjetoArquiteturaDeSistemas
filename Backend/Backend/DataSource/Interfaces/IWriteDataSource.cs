namespace Backend.DataSource.Interfaces
{
    public interface IWriteDataSource<T>
    {
        Task UpdateAsync(T entity);
    }
}
