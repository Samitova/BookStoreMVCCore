using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.ShopService.PaginationService
{
    public static class PaginationService
    {       
        public static int ProccessPageSize(int pageSize, HttpContext context)
        {           
            bool isPageSizeInSession = context.Session.TryGetValue("PageSize", out _);

            if (!isPageSizeInSession)
            {
                pageSize = 4;
                context.Session.SetString("PageSize", pageSize.ToString());
            }
            else if (pageSize == 0)
            {
                pageSize = int.Parse(context.Session.GetString("PageSize"));
            }
            else
            {
                context.Session.SetString("PageSize", pageSize.ToString());
            }
            return pageSize;
        }
    }
}
