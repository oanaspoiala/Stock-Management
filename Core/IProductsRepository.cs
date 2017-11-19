using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core
{
    public interface IProductsRepository
    {
        void Create(Product product);

        IReadOnlyList<Product> Get();

        Product Get(Guid id);

        void Update(Product product);

        void Delete(Guid id);
    }
}
