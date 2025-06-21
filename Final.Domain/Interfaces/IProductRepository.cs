using Final.Domain.Common;
using Final.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
    }
}
