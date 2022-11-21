﻿using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Views.Shared.Components.SearchBar
{
    public class SearchBarViewComponent: ViewComponent
    {
        public SearchBarViewComponent()
        {
        }

        public IViewComponentResult Invoke(SearchPager searchPager) 
        {
            return View("Default", searchPager);
        }
    }
}
