using Electronics.Application.Interface;
using Electronics.Application.ProductImage;
using Electronics.Domain.Entities;
using Microsoft.AspNetCore.Hosting;

namespace Electronics.Infrastructure.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IBaseRepository<ProductImage> _repository;
        private readonly IHostingEnvironment _env;

        public ProductImageService(IBaseRepository<ProductImage> repository, IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task AddAsync(ProductImageDto dto)
        {
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var file in dto.Images)
                {

                    var folderPath = Path.Combine(_env.WebRootPath, "ProductImages");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);


                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folderPath, fileName);

                    // Save file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Save record to database
                    var entity = new ProductImage
                    {
                        ProductId = dto.ProductId,
                        ImagePath = Path.Combine("ProductImages", fileName).Replace("\\", "/")
                    };

                    await _repository.AddAsync(entity);
                }
            }
        }

        public async Task<IEnumerable<ProductImageDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities.Select(entity => new ProductImageDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ImagePath = entity.ImagePath
            });
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return;

            // Delete image file from wwwroot folder if it exists
            if (!string.IsNullOrEmpty(entity.ImagePath))
            {
                var relativePath = entity.ImagePath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                var fullPath = Path.Combine(_env.WebRootPath, relativePath); if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }

            await _repository.DeleteAsync(id);
        }





        public async Task<IEnumerable<ProductImageDto>> GetByIdAsync(int id)
        {
            // Fetch all images (assuming GetAllAsync fetches all images in the repository)
            var entities = await _repository.GetAllAsync();

            // Filter images for the specific ProductId
            var productImages = entities.Where(e => e.ProductId == id).ToList();

            // Return the list of product images as ProductImageDto
            return productImages.Select(entity => new ProductImageDto
            {
                Id = entity.Id,
                ProductId = entity.ProductId,
                ImagePath = entity.ImagePath
            }).ToList();
        }





        Task IProductImageService.UpdateAsync(ProductImageDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
