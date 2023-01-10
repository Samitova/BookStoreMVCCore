using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.DataContext;
using BookStore.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {

        public AuthorRepository(BookStoreContext context) : base(context)
        { }

        public override Author GetById(int? id)
        {
            Author authorDTO = GetAll(filter: x => x.Id == id, includeProperties: "Books").FirstOrDefault();
            return authorDTO;
        }
               
        public override async Task<Author> GetByIdAsync(int? id)
        {
            IEnumerable<Author> authorsList = await GetAllAsync(filter: x => x.Id == id, includeProperties: "Books");
            Author author = authorsList?.FirstOrDefault();
            return author;
        }
    }
}
