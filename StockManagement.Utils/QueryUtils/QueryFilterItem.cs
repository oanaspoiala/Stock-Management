namespace StockManagement.Utils.QueryUtils
{
    public class QueryFilterItem
    {
        public string FieldName { get; set; }

        public string FilterValue { get; set; }

        public string FilterOperator { get; set; }

        public string ParameterName { get; set; }

        public override string ToString()
        {
            return $"{FieldName} {FilterOperator} @{ParameterName}";
        }
    }
}
