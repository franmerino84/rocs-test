using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TycoonFactoryScheduler.Abstractions.Exceptions;
using TycoonFactoryScheduler.Abstractions.Persistence;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly TycoonFactorySchedulerContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TycoonFactorySchedulerContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual void Delete(object id)
        {
            TEntity? entityToDelete = _dbSet.Find(id);

            if (entityToDelete != null)
                Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                _dbSet.Attach(entityToDelete);

            _dbSet.Remove(entityToDelete);
        }

        public virtual IQueryable<TEntity> Get(
                            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                return orderBy(query);

            return query;
        }

        public virtual TEntity GetById(object id) =>
            _dbSet.Find(id) ?? throw new EntityNotFoundInDatabaseException(typeof(TEntity), id);       

        public virtual void Insert(TEntity entity) =>
            _dbSet.Add(entity);

        public virtual bool TryGetById(object id, out TEntity? entity)
        {
            entity = _dbSet.Find(id);

            return entity != null;
        }
        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
