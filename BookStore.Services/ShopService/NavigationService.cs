﻿using BookStore.Data.Models.ViewModels;
using BookStore.Services.ShopService.PaginationService;
using System.Collections.Generic;


namespace BookStore.Services.ShopService
{
    public class NavigationService
    {               
        public PaginationModel PaginationModel { get; set; }
        public PaginatedList<BookVM> PagedBooks { get; set; }

        public NavigationService()
        {          
        }

        public void SetNavigationService(string action, List<BookVM>  books, int currentPage, int pageSize, string searchText, string sortExpression)
        {
            PagedBooks = new PaginatedList<BookVM>(books, currentPage, pageSize);   
            PaginationModel = new PaginationModel(action, PagedBooks.TotalRecord, currentPage, sortExpression, searchText, pageSize);
        }       
    }
}