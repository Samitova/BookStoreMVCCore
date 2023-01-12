using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.ViewModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Publisher = BookStore.DataAccess.Models.Publisher;

namespace BookStore.Services.Managers
{
    public class PublisherManager : IPublisherManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public PublisherManager(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public void AddPublisher(PublisherVM publisher)
        {
            try
            {
                _repository.Publishers.Add(_mapper.Map<Publisher>(publisher));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public void UpdatePublisher(PublisherVM publisher)
        {
            try
            {
                _repository.Publishers.Update(_mapper.Map<Publisher>(publisher));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public void DeletePublisher(int id)
        {
            try
            {
                _repository.Publishers.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task<IEnumerable<PublisherVM>> GetAllPublishersAsync()
        {
            IEnumerable<Publisher> publishersList = await _repository.Publishers.GetAllAsync(orderBy: x => x.OrderBy(y => y.PublisherName));
            return _mapper.Map<IEnumerable<PublisherVM>>(publishersList);
        }

        public PublisherVM GetPublisherById(int id)
        {
            Publisher publisher =  _repository.Publishers.GetById(id);
            return _mapper.Map<PublisherVM>(publisher);
        }

        public async Task<PublisherVM> GetPublisherByIdAsync(int? id)
        {
            Publisher publisher = await _repository.Publishers.GetByIdAsync(id);
            return _mapper.Map<PublisherVM>(publisher);
        }
    }
}
