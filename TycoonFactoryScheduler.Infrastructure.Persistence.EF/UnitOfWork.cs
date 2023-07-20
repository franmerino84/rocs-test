using Microsoft.EntityFrameworkCore;
using TycoonFactoryScheduler.Abstractions.Persistence;
using TycoonFactoryScheduler.Abstractions.Persistence.Activities;
using TycoonFactoryScheduler.Abstractions.Persistence.Workers;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Activities;
using TycoonFactoryScheduler.Infrastructure.Persistence.EF.Repositories.Workers;

namespace TycoonFactoryScheduler.Infrastructure.Persistence.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TycoonFactorySchedulerContext _context;

        public UnitOfWork(TycoonFactorySchedulerContext context)
        {
            _context = context;
            Activities = new ActivitiesRepository(_context);
            Workers = new WorkersRepository(_context);
        }

        public UnitOfWork(DbContextOptions<TycoonFactorySchedulerContext> options) : this(new TycoonFactorySchedulerContext(options)) { }

        public IActivitiesRepository Activities { get; private set; }
        public IWorkersRepository Workers { get; private set; }

        public void Save() =>
            _context.SaveChanges();

        public async Task SaveAsync() =>
            await _context.SaveChangesAsync();


        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
