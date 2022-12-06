﻿using BookStore.Data.Models.Interfaces;
using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.DataBaseService.Interfaces
{
    public interface IBookCommentsRepository : IRepositoryBase<BookCommentDTO>
    {
    }
}