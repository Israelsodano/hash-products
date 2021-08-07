using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hash.Products.Domain.Repository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}