using Electronics.Application.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Application.ProductImage
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageDto>> GetAllAsync();
        Task AddAsync(ProductImageDto dto);
        Task<IEnumerable<ProductImageDto>> GetByIdAsync(int productId);
        Task UpdateAsync(ProductImageDto dto);
        Task DeleteAsync(int id);
    }
}
