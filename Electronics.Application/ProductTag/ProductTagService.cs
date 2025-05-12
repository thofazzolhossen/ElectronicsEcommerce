using Electronics.Application.Interface;

namespace Electronics.Application.ProductTag
{
    public class ProductTagService : IProductTagService
    {
        private readonly IBaseRepository<Domain.Entities.ProductTag> _repository;

        public ProductTagService(IBaseRepository<Domain.Entities.ProductTag> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductTagDto>> GetAllAsync()
        {
            var tags = await _repository.GetAllAsync();
            return tags.Select(t => new ProductTagDto { Id = t.Id, Name = t.Name });
        }

        public async Task AddAsync(ProductTagDto dto)
        {
            var entity = new Domain.Entities.ProductTag { Name = dto.Name };
            await _repository.AddAsync(entity);
        }

        public async Task<ProductTagDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return new ProductTagDto { Id = entity.Id, Name = entity.Name };
        }

        public async Task UpdateAsync(ProductTagDto dto)
        {
            var entity = new Domain.Entities.ProductTag { Id = dto.Id, Name = dto.Name };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
