namespace TycoonFactoryScheduler.Abstractions.Persistence
{
    public interface IGenericWriteRepository<in TEntity> where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}
