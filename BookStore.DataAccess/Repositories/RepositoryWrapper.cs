﻿using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using System;


namespace BookStore.DataAccess.Repositories
{
    public class RepositoryWrapper : IDisposable, IRepositoryWrapper
    {
        private BookStoreContext _context;
        public RepositoryWrapper(BookStoreContext context)
        {
            _context = context;
        }

        private bool _disposed = false;     
        private IBookRepository _booksRepository;
        private IAuthorRepository _authorsRepository;
        private IPublisherRepository _publishersRepository;
        private ICategoryRepository _categoriesRepository;
        private IBookCommentsRepository _bookCommentsRepository;
        public IBookRepository Books
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

        public IBookCommentsRepository BookComments
        {
            get
            {
                if (_bookCommentsRepository == null)
                {
                    _bookCommentsRepository = new BookCommentsRepository(_context);
                }
                return _bookCommentsRepository;
            }
        }

        public IAuthorRepository Authors
        {
            get
            {
                if (_authorsRepository == null)
                {
                    _authorsRepository = new AuthorRepository(_context);
                }
                return _authorsRepository;
            }
        }

        public IPublisherRepository Publishers
        {
            get
            {
                if (_publishersRepository == null)
                {
                    _publishersRepository = new PublisherRepository(_context);
                }
                return _publishersRepository;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (_categoriesRepository == null)
                {
                    _categoriesRepository = new CategoryRepository(_context);
                }
                return _categoriesRepository;
            }
        }       

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
