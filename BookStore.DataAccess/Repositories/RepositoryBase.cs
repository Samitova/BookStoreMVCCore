using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;


namespace BookStore.DataAccess.Repositories
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
            try
            {
                _table.Add(entity);
                SaveChanges();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);                
            }                  
        }

        public void Update(T entity)
        {
            try
            {
                _context.ChangeTracker.Clear();
                _context.Update(entity);
                SaveChanges();                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }                       
        }

        public void Delete(int id)
        {
            try
            {
                T entityToDelete = _table.Find(id);
                Delete(entityToDelete);
                SaveChanges();               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            
        }

        public void Delete(T entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _table.Attach(entity);
                }
                _table.Remove(entity);
                SaveChanges();               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }           
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

        public virtual async Task<T> GetByIdAsync(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual T GetById(int? id)
        {
            return  _table.Find(id);
        }
        #endregion
        public void SaveChanges()
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
