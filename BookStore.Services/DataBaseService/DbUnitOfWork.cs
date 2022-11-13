using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using BookStore.Services.DataBaseService.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService
{
    public class DbUnitOfWork : IDisposable
    {
        private BookStoreContext _context;
        public DbUnitOfWork(BookStoreContext context)
        {
            _context = context;
        }

        private BookRepository _booksRepository;
        private EfRepository<Author> _authorsRepository;
        private EfRepository<Publisher> _publishersRepository;

        public BookRepository BooksRepository
        {
            get
            {
                if (_booksRepository == null)
                {
                    _booksRepository = new BookRepository(_context);
                }
                return _booksRepository;
            }
        }

        public EfRepository<Author> AuthorsRepository
        {
            get
            {
                if (_authorsRepository == null)
                {
                    _authorsRepository = new EfRepository<Author>(_context);
                }
                return _authorsRepository;
            }
        }

        public EfRepository<Publisher> PublishersRepository
        {
            get
            {
                if (_publishersRepository == null)
                {
                    _publishersRepository = new EfRepository<Publisher>(_context);
                }
                return _publishersRepository;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
