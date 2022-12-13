using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BookStore.Services.ShopService.SortingService
{
    public enum SortOrder {Ascending=0, Descending =1};

    [DataContract]
    public class SortModel
    {
        private string _upIcon = "fa-solid fa-caret-up";
        private string _downIcon = "fa-solid fa-caret-down";

        [DataMember]
        public string SortedProperty { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public SortOrder SortedOrder { get; set; }

        [DataMember]
        public string SortExpression { get; set; }

        [DataMember]
        public List<SortableColumn> SortableColumns = new List<SortableColumn>();

        public SortModel()
        {          
        }      

        public void AddColumn(string columnName, string sortOrderExpression, bool isDefaultColumn = false) 
        {
            SortableColumn checkColumn = this.SortableColumns.Where(c => c.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();
            if (checkColumn == null)
            { 
                SortableColumns.Add(new SortableColumn() { ColumnName = columnName, SortExpression = sortOrderExpression});
            }

            if (isDefaultColumn == true || SortableColumns.Count == 1)
            { 
                SortedProperty = columnName;
                SortedOrder = SortOrder.Ascending;
            }
        }

        public SortableColumn GetColumn(string columnName) 
        {
            SortableColumn checkColumn = this.SortableColumns.Where(c => c.ColumnName.ToLower() == columnName.ToLower()).SingleOrDefault();
            if (checkColumn == null)
            {
                SortableColumns.Add(new SortableColumn() { ColumnName = columnName });
            }
            return checkColumn;
        }

        public void ApplySort(string action, string sortExpression)
        {
            Action = action;
            if (string.IsNullOrEmpty(sortExpression))
            {
                sortExpression = this.SortedProperty.ToLower();
            }

            SortExpression = sortExpression;

            foreach (SortableColumn sortableColumn in this.SortableColumns)
            {
                sortableColumn.SortIcon = "";                

                if (sortExpression == sortableColumn.ColumnName.ToLower())
                {
                    this.SortedOrder = SortOrder.Ascending;
                    this.SortedProperty = sortableColumn.ColumnName;                    
                    sortableColumn.SortIcon = _downIcon;
                    sortableColumn.SortExpression = sortableColumn.ColumnName + "_desc";
                }
                else if (sortExpression == sortableColumn.ColumnName.ToLower() + "_desc")
                {
                    this.SortedOrder = SortOrder.Descending;
                    this.SortedProperty = sortableColumn.ColumnName;
                    sortableColumn.SortIcon = _upIcon;
                    sortableColumn.SortExpression = sortableColumn.ColumnName;
                }
            }
        }

        public void InitSortModel(Dictionary<string, string> properties)
        {            
            foreach (var property in properties)
            {
                AddColumn(property.Key, property.Value);                
            }  
        }
    }

    public class SortableColumn
    {        
        public string ColumnName { get; set; }
        public string SortExpression { get; set; }
        public string SortIcon { get; set; }
    }
}
