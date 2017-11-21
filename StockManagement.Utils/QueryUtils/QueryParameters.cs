using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace StockManagement.Utils.QueryUtils
{
    public class QueryParameters
    {
        public QueryParameters()
        {
            Filters = new List<QueryFilterItem>();
            SortItems = new List<QuerySortItem>();
        }
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public List<QueryFilterItem> Filters { get; set; }

        public List<QuerySortItem> SortItems { get; set; }

        public string GetWhereClause()
        {
            if (Filters == null || !Filters.Any())
            {
                return string.Empty;
            }
            return $"WHERE {string.Join(" AND ", Filters.Select(x => x.ToString()))}";
        }

        public string GetOrderByClause()
        {
            if (SortItems == null || !SortItems.Any())
            {
                return string.Empty;
            }

            return $"ORDER BY {string.Join(", ", SortItems.Select(x => x.ToString()))}";
        }

        public string GetPaginationClause()
        {
            return $"OFFSET {(PageNumber - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";
        }

        public object GetParameters()
        {
            if (Filters == null || !Filters.Any())
            {
                return null;
            }
            var dict = new Dictionary<string, object>();
            IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
            foreach (var filter in Filters)
            {
                var kvp = new KeyValuePair<string, object>(
                    filter.ParameterName,
                    string.Compare(filter.FilterOperator, "like", StringComparison.OrdinalIgnoreCase) == 0
                        ? $"%{filter.FilterValue}%"
                        : filter.FilterValue);
                eo.Add(kvp);
            }

            return eo;
        }
    }
}
