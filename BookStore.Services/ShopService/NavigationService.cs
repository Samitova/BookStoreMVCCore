using BookStore.Data.Models.ViewModels;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SotrOrderingService;
using System.Collections.Generic;


namespace BookStore.Services.ShopService
{
    public class NavigationService
    {
        public SearchBar SearchBar { get; set; }
        public SortModel SortModel { get; set; }
        public PaginationModel PaginationModel { get; set; }
        public PaginatedList<BookVM> PagedBooks { get; set; }

        public NavigationService()
        {
            SortModel = new SortModel();
            InitSortModel();   
        }

        public void SetNavigationService(List<BookVM>  books, int currentPage, int pageSize, string searchText, string sortExpression)
        {
            PagedBooks = new PaginatedList<BookVM>(books, currentPage, pageSize);
            PaginationModel = new PaginationModel(PagedBooks.TotalRecord, currentPage, sortExpression, searchText, pageSize);
        }
        public void SetSearchBar(string searchText)
        {            
            SearchBar = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = searchText };
        }
        public void SetOrderModel(string sortExpression)
        {
            SortModel.ApplySort(sortExpression);
        }        
        private void InitSortModel()
        {
            SortModel.AddColumn("title", "title");
            SortModel.AddColumn("price", "price_desc");
            SortModel.AddColumn("authorfullname", "authorfullname");
            SortModel.AddColumn("rating", "rating_desc");
            SortModel.AddColumn("bestsellers", "bestsellers_desc");
            SortModel.AddColumn("novelties", "novelties_desc");
        }
    }
}
