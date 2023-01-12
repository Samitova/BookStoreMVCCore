using BookStore.ViewModelData.Attributes;
using BookStore.ViewModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BookStore.Services.ShopService.SortingService
{
    public static class SortService
    {
        public static IEnumerable<BookVM> SortBooks(IEnumerable<BookVM> books, SortModel sortModel)
        {
            Type type = typeof(BookVM);
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.GetCustomAttribute<OrderKeyAttribute>()?.Key == sortModel.SortedProperty)
                {
                    return sortModel.SortedOrder == SortOrder.Ascending ? books.OrderBy(prop.GetValue).ToList()
                                                            : books.OrderByDescending(prop.GetValue).ToList();
                }
            }
            return books;
        }

        public static SortModel SetSortModel(string action, string sortExpression, Dictionary<string, string> sortedProperties) 
        {
            SortModel sortModel = new SortModel();
            sortModel.InitSortModel(sortedProperties);
            sortModel.ApplySort(action, sortExpression);           
            return sortModel;
        }
    }
}
