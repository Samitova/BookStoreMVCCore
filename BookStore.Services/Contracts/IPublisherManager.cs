using BookStore.DataAccess.Models;
using BookStore.ViewModelData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Contracts
{
    public interface IPublisherManager
    {
        PublisherVM GetPublisherById(int id);
        Task<PublisherVM> GetPublisherByIdAsync(int? id);
        Task<IEnumerable<PublisherVM>> GetAllPublishersAsync();
        void DeletePublisher(int id);
        void AddPublisher(PublisherVM publisher);
        void UpdatePublisher(PublisherVM publisher);
    }
}
