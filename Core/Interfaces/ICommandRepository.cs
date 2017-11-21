using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementStocks.Core.Interfaces
{
    public interface ICommandRepository<T>
    {
        void Create(T product);

        void Update(T product);

        void Delete(Guid id);
    }
}
