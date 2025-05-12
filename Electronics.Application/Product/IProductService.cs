namespace Electronics.Application.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task AddAsync(ProductDto dto);
        Task<ProductDto> GetByIdAsync(int id);
        Task UpdateAsync(ProductDto dto);
        Task DeleteAsync(int id);
    }
}
