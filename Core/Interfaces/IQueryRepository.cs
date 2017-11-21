using StockManagement.Utils.QueryUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementStocks.Core.Interfaces
{
    public interface IQueryRepository<T>
    {
        IReadOnlyList<T> Get();

        IReadOnlyList<T> Get(QueryParameters queryParameters);

        T Get(Guid id);
    }
}
