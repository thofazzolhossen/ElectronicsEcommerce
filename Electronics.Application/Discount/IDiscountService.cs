using Electronics.Application.ProductTag;

namespace Electronics.Application.Discount
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetAllAsync();
        Task AddAsync(DiscountDto dto);
        Task<DiscountDto> GetByIdAsync(int id);
        Task UpdateAsync(DiscountDto dto);
        Task DeleteAsync(int id);
    }
}
