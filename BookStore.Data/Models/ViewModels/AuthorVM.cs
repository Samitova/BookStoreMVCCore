using BookStore.Data.Models.ModelsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

namespace BookStore.Data.Models.ViewModels
{
    public class AuthorVM : BaseEntity
    {
        [DisplayName("Author")]
        public string FullName { get; set; }        
        public string PhotoPath { get; set; }
        public string Biography { get; set; }
        public double RateValue { get; set; } = 0;
        public int RateCount { get; set; } = 0;
        public List<BookVM> Books { get; set; }
        public AuthorVM()
        {
        }       
    }
}
