using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfBusinessLayer
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        DbSet<TEntity> Set();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
