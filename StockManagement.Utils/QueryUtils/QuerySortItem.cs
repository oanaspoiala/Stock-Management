using System;

namespace StockManagement.Utils.QueryUtils
{
    public class QuerySortItem
    {
        public string FieldName { get; set; }

        public bool Descending { get; set; }

        public override string ToString()
        {
            var direction = Descending ? " DESC" : string.Empty;
            return $@"{FieldName}{direction}";
        }
    }
}
