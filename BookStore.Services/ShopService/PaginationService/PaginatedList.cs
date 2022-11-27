using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Services.ShopService.PaginationService
{
    public class PaginatedList<T> : List<T>
    {
        public int TotalRecord { get; private set; }

        public PaginatedList()
        {                
        }
        public PaginatedList(List<T> source, int pageIndex, int pageSize)
        {
            TotalRecord = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            this.AddRange(items);
        }
    }
}
