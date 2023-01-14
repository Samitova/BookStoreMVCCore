using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Managers
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public AuthorManager(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public void AddAuthor(AuthorViewModel author)
        {
            try
            {
                _repository.Authors.Add(_mapper.Map<Author>(author));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        public void UpdateAuthor(AuthorViewModel author)
        {
            try
            {
                _repository.Authors.Update(_mapper.Map<Author>(author));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public void DeleteAuthor(int id)
        {
            try
            {
                _repository.Authors.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }                        
        }

        public async Task<IEnumerable<AuthorViewModel>> GetAllAuthorsAsync()
        {
            IEnumerable<Author> authorsList = await _repository.Authors.GetAllAsync(orderBy: x => x.OrderBy(y => y.FullName));
            return _mapper.Map<IEnumerable<AuthorViewModel>>(authorsList);
        }

        public AuthorViewModel GetAuthorById(int id)
        {
            Author author =  _repository.Authors.GetById(id);
            return _mapper.Map<AuthorViewModel>(author);
        }

        public async Task<AuthorViewModel> GetAuthorByIdAsync(int? id)
        {
            Author author = await _repository.Authors.GetByIdAsync(id);           
            return _mapper.Map<AuthorViewModel>(author);
        }

        public async Task<AuthorViewModel> GetAuthorWithBooksAsync(int id, SortModel sortModel)
        {
            Author author = await _repository.Authors.GetByIdAsync(id);
            AuthorViewModel authorVM = _mapper.Map<AuthorViewModel>(author);
            authorVM.Books = SortService.SortBooks(authorVM.Books, sortModel);
            return authorVM;
        }


    }
}
