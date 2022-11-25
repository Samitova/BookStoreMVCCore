using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Data.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderKeyAttribute: Attribute
    {
        public string Key { get; set; }

        public OrderKeyAttribute(string key)
        {
            Key = key; 
        }
    }
}
