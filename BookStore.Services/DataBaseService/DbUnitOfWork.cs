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
        private EfRepository<AuthorDTO> _authorsRepository;
        private EfRepository<PublisherDTO> _publishersRepository;
        private EfRepository<CategoryDTO> _categoriesRepository;


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

        public EfRepository<AuthorDTO> AuthorsRepository
        {
            get
            {
                if (_authorsRepository == null)
                {
                    _authorsRepository = new EfRepository<AuthorDTO>(_context);
                }
                return _authorsRepository;
            }
        }

        public EfRepository<PublisherDTO> PublishersRepository
        {
            get
            {
                if (_publishersRepository == null)
                {
                    _publishersRepository = new EfRepository<PublisherDTO>(_context);
                }
                return _publishersRepository;
            }
        }

        public EfRepository<CategoryDTO> CategoriesRepository
        {
            get
            {
                if (_categoriesRepository == null)
                {
                    _categoriesRepository = new EfRepository<CategoryDTO>(_context);
                }
                return _categoriesRepository;
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
