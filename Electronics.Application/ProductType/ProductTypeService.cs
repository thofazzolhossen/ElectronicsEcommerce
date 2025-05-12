using Electronics.Application.Interface;
using Electronics.Application.ProductType;
using Electronics.Domain.Entities;

namespace Electronics.Infrastructure.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IBaseRepository<ProductType> _repository;

        public ProductTypeService(IBaseRepository<ProductType> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return types.Select(t => new ProductTypeDto { Id = t.Id, Name = t.Name });
        }

        public async Task AddAsync(ProductTypeDto dto)
        {
            var entity = new ProductType { Name = dto.Name };
            await _repository.AddAsync(entity);
        }
        public async Task<ProductTypeDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return new ProductTypeDto { Id = entity.Id, Name = entity.Name };
        }

        public async Task UpdateAsync(ProductTypeDto dto)
        {
            var entity = new ProductType { Id = dto.Id, Name = dto.Name };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}
