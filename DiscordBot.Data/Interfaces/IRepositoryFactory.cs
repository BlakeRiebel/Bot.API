namespace DiscordBot.Data.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
        IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class;
        IRepositoryReadOnly<T> GetReadOnlyRepository<T>() where T : class;
        IRepositoryReadOnlyAsync<T> GetReadOnlyRepositoryAsync<T>() where T : class;
    }
}