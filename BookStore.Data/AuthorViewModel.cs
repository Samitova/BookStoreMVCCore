using BookStore.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

namespace BookStore.ViewModelData
{
    public class AuthorViewModel : BaseEntity
    {
        public AuthorViewModel()
        { }

        [DisplayName("Author")]
        public string FullName { get; set; }
        public string PhotoPath { get; set; }      
        public string Biography { get; set; }
        public IEnumerable<BookViewModel> Books { get; set; }
        public IFormFile AuthorImage { get; set; }
      
    }
}
