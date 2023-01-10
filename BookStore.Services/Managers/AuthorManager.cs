using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.ViewModelData;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Services.Managers
{
    public class AuthorManager : IAuthorManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public AuthorManager(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public void AddAuthor(AuthorVM author)
        {
            _repository.Authors.Add(_mapper.Map<Author>(author));
        }

        public void UpdateAuthor(AuthorVM author)
        {
            _repository.Authors.Update(_mapper.Map<Author>(author));
        }

        public Author DeleteAuthor(int? id)
        {
            return _repository.Authors.Delete(id);             
        }

        public async Task<IEnumerable<AuthorVM>> GetAllAuthorsAsync()
        {
            IEnumerable<Author> authorsList = await _repository.Authors.GetAllAsync(orderBy: x => x.OrderBy(y => y.FullName));
            return _mapper.Map<IEnumerable<AuthorVM>>(authorsList);
        }

        public async Task<AuthorVM> GetAuthorByIdAsync(int? id)
        {
            Author author = await _repository.Authors.GetByIdAsync(id);           
            return _mapper.Map<AuthorVM>(author);
        }

      
    }
}
