using System.Threading.Tasks;

namespace EfBusinessLayer
{
    public interface IBusinessLayer<TEntity>
        where TEntity : class
    {
        Task AddAsync(TEntity entity);
    }
}
