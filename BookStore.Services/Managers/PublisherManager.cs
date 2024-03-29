﻿using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.ViewModelData;
using Microsoft.EntityFrameworkCore;
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

        public void AddPublisher(PublisherViewModel publisher)
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

        public void UpdatePublisher(PublisherViewModel publisher)
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
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PublisherViewModel>> GetAllPublishersAsync()
        {
            IEnumerable<Publisher> publishersList = await _repository.Publishers.GetAllAsync(orderBy: x => x.OrderBy(y => y.PublisherName));
            return _mapper.Map<IEnumerable<PublisherViewModel>>(publishersList);
        }

        public PublisherViewModel GetPublisherById(int id)
        {
            Publisher publisher =  _repository.Publishers.GetById(id);
            return _mapper.Map<PublisherViewModel>(publisher);
        }

        public async Task<PublisherViewModel> GetPublisherByIdAsync(int? id)
        {
            Publisher publisher = await _repository.Publishers.GetByIdAsync(id);
            return _mapper.Map<PublisherViewModel>(publisher);
        }
    }
}
