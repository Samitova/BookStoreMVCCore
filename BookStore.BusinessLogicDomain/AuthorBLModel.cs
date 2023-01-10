using BookStore.DataAccess.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.BusinessLogicDomain
{
    public class AuthorBLModel
    {
        public AuthorBLModel()
        { }

        public int Id { get; set; }
        public byte[] Timestamp { get; set; }
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
        public string Biography { get; set; }
        public List<Book> Books { get; set; }
       
    }
}
