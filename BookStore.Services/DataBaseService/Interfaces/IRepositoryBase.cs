using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data.Models.ModelsDTO;

namespace BookStore.Data.Models.Interfaces
{
    public interface IRepositoryBase<T> 
    {
        Task<T> GetById(int id);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        Task Update(T entity);
        Task Remove(T entity);        
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties);
        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<T, bool>> filter);
        Task<bool> Any(Expression<Func<T, bool>> filter);
    }
}
