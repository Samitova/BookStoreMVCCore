using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.DataBaseService.Interfaces
{
    public interface IAuthorRepository : IRepositoryBase<AuthorDTO>
    {
        AuthorDTO GetAuthorById(int id);
    }
}
