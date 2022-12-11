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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T: class
    {

        #region Fields
        private readonly BookStoreContext _context;
        private readonly DbSet<T> _table;       

        #endregion

        public RepositoryBase(BookStoreContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        #region Async Methods
     
        public void Add(T entity)
        {
            _table.Add(entity);
            SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            SaveChanges();
        }

        public void Delete(int id)
        {
            T entityToDelete = _table.Find(id);
            Delete(entityToDelete);
            SaveChanges();
        }
        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
            SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,  string includeProperties = "")
        {
            IQueryable<T> query = _table;

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

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _table;

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
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<T> GetByIdAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public virtual T GetById(int? id)
        {
            return  _table.Find(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await _table.FirstOrDefaultAsync(filter);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
           return  _table.FirstOrDefault(filter);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _table.AnyAsync(filter);
        }
        public bool Any(Expression<Func<T, bool>> filter)
        {
            return _table.Any(filter);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return await _table.CountAsync();
            }          
            return await _table.CountAsync(filter);
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            if (filter == null)
            {
                return _table.Count();
            }
            return _table.Count(filter);
        }

        #endregion
        internal void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
