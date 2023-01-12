using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.Services.Contracts;

namespace BookStore.Services.Managers
{
    public class ShopManager:IShopManager
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        private IBookManager _bookManager;
        private IAuthorManager _authorManager;
        private IPublisherManager _publisherManager;
        private ICategoryManager _categoryManager;

        public ShopManager(IRepositoryWrapper repository, IMapper mapper)
        {
           _repository = repository;
            _mapper = mapper;
        }

        public IBookManager BookManager
        {
            get
            {
                if (_bookManager == null)
                {
                    _bookManager = new BookManager(_repository, _mapper);
                }
                return _bookManager;
            }
        }

        public IAuthorManager AuthorManager
        {
            get
            {
                if (_authorManager == null)
                {
                    _authorManager = new AuthorManager(_repository, _mapper);
                }
                return _authorManager;
            }
        }

        public IPublisherManager PublisherManager
        {
            get
            {
                if (_publisherManager == null)
                {
                    _publisherManager = new PublisherManager(_repository, _mapper);
                }
                return _publisherManager;
            }
        }

        public ICategoryManager CategoryManager
        {
            get
            {
                if (_categoryManager == null)
                {
                    _categoryManager = new CategoryManager(_repository, _mapper);
                }
                return _categoryManager;
            }
        }
    }
}
