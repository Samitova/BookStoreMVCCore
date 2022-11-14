﻿using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data.Models.Interfaces
{
    public interface IBookRepository:IAsyncRepository<BookDTO>
    {
        Task<BookDTO> GetByTitle(string title);
    }
}
