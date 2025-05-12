using Electronics.Application.Interface;
using Electronics.Application.ProductTag;
using Electronics.Application.ProductType;
using Electronics.Application.Discount;
using Microsoft.AspNetCore.Http;
using Electronics.Domain.Entities;
using Electronics.Application.ProductImage;

namespace Electronics.Application.Product
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Domain.Entities.Product> _repository;
        private readonly IBaseRepository<Domain.Entities.ProductImage> _productImageRepository;
        private readonly IProductTypeService _productTypeService;
        private readonly IProductTagService _productTagService;
        private readonly IDiscountService _discountService;
        private readonly IProductImageService _productImageService;

        public ProductService(
            IBaseRepository<Domain.Entities.Product> repository,
            IBaseRepository<Domain.Entities.ProductImage> productImageRepository,
            IProductTypeService productTypeService,
            IProductTagService productTagService,
            IDiscountService discountService,
            IProductImageService productImageService)
        {
            _repository = repository;
            _productImageRepository = productImageRepository;
            _productTypeService = productTypeService;
            _productTagService = productTagService;
            _discountService = discountService;
            _productImageService = productImageService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            var types = await _productTypeService.GetAllAsync();
            var tags = await _productTagService.GetAllAsync();
            var discounts = await _discountService.GetAllAsync();
            var allImages = await _productImageRepository.GetAllAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = (decimal)p.Price,
                Quantity = p.Quantity,
                ProductTypeId = p.ProductTypeId,
                ProductTagId = p.ProductTagId,
                DiscountId = p.DiscountId,
                Description = p.Description,
                Specification = p.Specification,
                ProductTypeName = types.FirstOrDefault(t => t.Id == p.ProductTypeId)?.Name,
                ProductTagName = tags.FirstOrDefault(t => t.Id == p.ProductTagId)?.Name,
                DiscountName = discounts.FirstOrDefault(d => d.Id == p.DiscountId)?.Name,

                ImageUrls = allImages
                    .Where(img => img.ProductId == p.Id)
                    .Select(img => img.ImagePath)
                    .ToList()
            });
        }


        public async Task<ProductDto> GetByIdAsync(int id)
        {
            // Fetch all product images for the given product ID
            var productImages = await _productImageService.GetByIdAsync(id);  // Now this returns a list of ProductImageDto
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null) return null;

            // Retrieve related data like ProductType, ProductTag, Discount
            var type = await _productTypeService.GetByIdAsync(entity.ProductTypeId);
            var tag = await _productTagService.GetByIdAsync(entity.ProductTagId);
            var discount = entity.DiscountId.HasValue && entity.DiscountId.Value > 0
                ? await _discountService.GetByIdAsync(entity.DiscountId.Value)
                : null;

            // Return the product data along with all associated image URLs
            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = (decimal)entity.Price,
                Quantity = entity.Quantity,
                ProductTypeId = entity.ProductTypeId,
                ProductTagId = entity.ProductTagId,
                DiscountId = entity.DiscountId,
                Description = entity.Description,
                Specification = entity.Specification,
                ProductTypeName = type?.Name,
                ProductTagName = tag?.Name,
                DiscountName = discount?.Name,
                // Here we ensure all image URLs are returned
                ImageUrls = productImages?.Select(img => img.ImagePath).ToList() ?? new List<string>()
            };
        }



        public async Task AddAsync(ProductDto dto)
        {
            var entity = new Domain.Entities.Product
            {
                Name = dto.Name,
                Price = (double)dto.Price,
                Quantity = dto.Quantity,
                ProductTypeId = dto.ProductTypeId,
                ProductTagId = dto.ProductTagId,
                Description = dto.Description,
                Specification = dto.Specification,
                DiscountId = dto.DiscountId ?? null
            };

            await _repository.AddAsync(entity);

            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var image in dto.Images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var uploadPath = Path.Combine("wwwroot/uploads", fileName);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var productImage = new Domain.Entities.ProductImage
                        {
                            ProductId = entity.Id,
                            ImagePath = "/uploads/" + fileName
                        };

                        await _productImageRepository.AddAsync(productImage);
                    }
                }
            }
        }

        public async Task UpdateAsync(ProductDto dto)
        {
            var entity = new Domain.Entities.Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = (double)dto.Price,
                Quantity = dto.Quantity,
                ProductTypeId = dto.ProductTypeId,
                ProductTagId = dto.ProductTagId,
                Description = dto.Description,
                Specification = dto.Specification,
                DiscountId = dto.DiscountId ?? 0
            };

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
