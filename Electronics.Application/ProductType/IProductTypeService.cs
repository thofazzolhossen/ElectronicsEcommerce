using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Application.ProductType
{
    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeDto>> GetAllAsync();
        Task AddAsync(ProductTypeDto productTypeDto);
        Task<ProductTypeDto> GetByIdAsync(int id);
        Task UpdateAsync(ProductTypeDto productTypeDto);
        Task DeleteAsync(int id);

    }
}
