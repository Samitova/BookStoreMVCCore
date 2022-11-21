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
        #region Async Methods

        T GetById(int id);
        Task<T> GetByIdAsync(int id);       
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");  
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);       
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);

        T FirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        int Count(Expression<Func<T, bool>> filter = null);
        bool Any(Expression<Func<T, bool>> filter);


        #endregion

        #region Changing Methods      

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        #endregion

    }
}
