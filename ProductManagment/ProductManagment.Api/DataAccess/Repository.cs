using Microsoft.EntityFrameworkCore;
using ProductManagment.Api.Interfaces.Repositories;
using ProductManagment.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagment.Api.DataAccess
{
    public abstract class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(T model)
        {
            await _dataContext.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _dataContext.Entry(model).State = EntityState.Deleted;
        }

        public async Task<T> Get(int id)
        {
            return await _dataContext.Set<T>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async IAsyncEnumerable<T> GetAllActive()
        {
            foreach(var model in await _dataContext.Set<T>().Where(m => m.IsActive).ToListAsync())
            {
                yield return model;
            }
        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
