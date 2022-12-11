using BookStore.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Interfaces
{
    public interface IRepositoryWrapper
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IPublisherRepository Publishers { get; }
        ICategoryRepository Categories { get; }
        IBookCommentsRepository BookComments { get; }      
    }
}
