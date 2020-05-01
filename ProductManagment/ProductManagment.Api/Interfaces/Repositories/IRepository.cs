using ProductManagment.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagment.Api.Interfaces.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<T> Get(int id);
        IAsyncEnumerable<T> GetAllActive();
        Task Add(T model);
        void Delete(T model);
        Task SaveChanges();
    }
}
