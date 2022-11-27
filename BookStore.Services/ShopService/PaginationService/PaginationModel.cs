using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BookStore.Services.ShopService.PaginationService
{
    public class PaginationModel
    {
        // Readonly properties
        public int TotalItems { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int PageStep { get; private set; } = 5;
        public int CurrentPage { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartRecord { get; private set; }
        public int EndRecord { get; private set; }

        // public properties
        public string Action { get; set; } = "Index";
        public string SearchText { get; set; }
        public string SortExpression { get; set; }

        public PaginationModel(int totalItems, int currentPage, string sortExpression, string searchText, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortExpression = sortExpression;
            SearchText = searchText;

            TotalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize); ;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (StartPage - 1);
                startPage = 1;
            }
            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 10)
                    startPage = endPage - 9;
            }
            StartRecord = (currentPage - 1) * pageSize + 1;
            EndRecord = StartRecord - 1 + pageSize;
            if (EndRecord > TotalItems)
                EndRecord = TotalItems;
            if (TotalItems == 0)
            {
                StartRecord = 0;
                EndRecord = 0;
                StartPage = 0;
                CurrentPage = 0;
            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }

        public List<SelectListItem> GetPaginationSize()
        {
            List<SelectListItem> pageSizeItem = new List<SelectListItem>();
            for (int lp = PageStep; lp < 20; lp += PageStep)
            {
                if (lp == this.PageSize)                 
                    pageSizeItem.Add(new SelectListItem(lp.ToString(), lp.ToString(), true));                
                else
                    pageSizeItem.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }
            return pageSizeItem;
        }
    }
}
