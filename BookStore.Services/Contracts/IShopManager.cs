using BookStore.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IShopManager
    {
        IBookManager BookManager { get; }
        IAuthorManager AuthorManager { get; }
        IPublisherManager PublisherManager { get; }
        ICategoryManager CategoryManager { get; }        
    }
}
