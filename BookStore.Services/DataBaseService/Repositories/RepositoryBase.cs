using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        #region Fields

        protected BookStoreContext Context;

        #endregion

        public RepositoryBase(BookStoreContext context)
        {
            Context = context;
        }

        #region Async Methods
     
        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            // In case AsNoTracking is used
            Context.Entry(entity).State = EntityState.Modified;            
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);            
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = Context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }     

        public async Task<T> GetByIdAsync(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }
        public T GetById(int id)
        {
            return  Context.Set<T>().Find(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(filter);
        }      

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await Context.Set<T>().AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await Context.Set<T>().CountAsync();
            }          
            return await Context.Set<T>().CountAsync(filter);
        }

        #endregion
    }
}
