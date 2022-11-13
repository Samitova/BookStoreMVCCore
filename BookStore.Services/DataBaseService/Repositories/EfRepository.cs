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
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {

        #region Fields

        protected BookStoreContext Context;

        #endregion

        public EfRepository(BookStoreContext context)
        {
            Context = context;
        }

        #region Public methods
        public async Task Add(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            // In case AsNoTracking is used
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        public async Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeProperties)
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

        public async Task<T> GetById(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> Any(Expression<Func<T, bool>> filter)
        {
            return await Context.Set<T>().AnyAsync(filter);
        }

        public async Task<int> CountAll()
        {
            return await Context.Set<T>().CountAsync();
        }

        public async Task<int> CountWhere(Expression<Func<T, bool>> filter)
        {
            return await Context.Set<T>().CountAsync(filter);
        }

        #endregion
    }
}
