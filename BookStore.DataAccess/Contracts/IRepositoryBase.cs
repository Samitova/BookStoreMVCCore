using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;


namespace BookStore.DataAccess.Contracts
{
    public interface IRepositoryBase<T> 
    {
        #region Methods
        T GetById(int? id);
        Task<T> GetByIdAsync(int? id);   
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = ""); 
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");      
        #endregion

        #region Changing Methods      

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);

        #endregion

    }
}
