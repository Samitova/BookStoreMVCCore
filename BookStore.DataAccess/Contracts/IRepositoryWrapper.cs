
namespace BookStore.DataAccess.Contracts
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
