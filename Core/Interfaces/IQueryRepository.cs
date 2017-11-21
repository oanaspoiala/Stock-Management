using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementStocks.Core.Interfaces
{
    public interface IQueryRepository<T>
    {
        IReadOnlyList<T> Get();

        T Get(Guid id);
    }
}
