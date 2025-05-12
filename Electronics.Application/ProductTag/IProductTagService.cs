using Electronics.Application.ProductTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Application.ProductTag
{
    public interface IProductTagService
    {
        Task<IEnumerable<ProductTagDto>> GetAllAsync();
        Task AddAsync(ProductTagDto dto);
        Task<ProductTagDto> GetByIdAsync(int id);
        Task UpdateAsync(ProductTagDto dto);
        Task DeleteAsync(int id);
    }

}
