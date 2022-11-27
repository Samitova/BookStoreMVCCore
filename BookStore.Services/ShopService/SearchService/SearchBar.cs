using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.ShopService.SearchService
{
    public class SearchBar
    {
        public SearchBar()
        {
        }

        public string SearchText { get; set; }
        public string Controler { get; set; }
        public string Action { get; set; }
    }
}
