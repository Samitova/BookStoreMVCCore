﻿using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Services.DataBaseService.Context;
using BookStore.Services.DataBaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services.DataBaseService.Repositories
{
    public class AuthorRepository : RepositoryBase<AuthorDTO>, IAuthorRepository
    {

        public AuthorRepository(BookStoreContext context) : base(context)
        { }

        public override AuthorDTO GetById(int id)
        {
            AuthorDTO authorDTO = GetAll(filter: x => x.Id == id, includeProperties: "Books").FirstOrDefault();
            return authorDTO;
        }
    }
}
