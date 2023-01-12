using System;

namespace BookStore.ViewModelData.Attributes
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
