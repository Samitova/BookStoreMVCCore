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
        PublisherViewModel GetPublisherById(int id);
        Task<PublisherViewModel> GetPublisherByIdAsync(int? id);
        Task<IEnumerable<PublisherViewModel>> GetAllPublishersAsync();
        void DeletePublisher(int id);
        void AddPublisher(PublisherViewModel publisher);
        void UpdatePublisher(PublisherViewModel publisher);
    }
}
