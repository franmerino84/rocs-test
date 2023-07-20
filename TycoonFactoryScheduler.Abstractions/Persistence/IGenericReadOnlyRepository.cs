using System.Linq.Expressions;

namespace TycoonFactoryScheduler.Abstractions.Persistence
{
    public interface IGenericReadOnlyRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        TEntity GetById(object id);
        bool TryGetById(object id, out TEntity? entity);
    }
}
