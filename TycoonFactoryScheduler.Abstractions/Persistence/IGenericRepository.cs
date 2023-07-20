namespace TycoonFactoryScheduler.Abstractions.Persistence
{
    public interface IGenericRepository<TEntity>: IGenericReadOnlyRepository<TEntity>, IGenericWriteRepository<TEntity>
        where TEntity : class
    {
       
    }
}