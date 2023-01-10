using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.ViewModelData
{
    public class CartVM
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
