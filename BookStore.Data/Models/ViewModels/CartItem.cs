using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.Models.ViewModels
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public int AvaliableQuantaty { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total
        {
            get
            {
                return Price * Quantity;
            }
        }
        public string Image { get; set; }
        //public IDictionary<int, int> Items { get; set; } = new Dictionary<int, int>();       
        //public decimal Amount { get; set; }       
    }
}
