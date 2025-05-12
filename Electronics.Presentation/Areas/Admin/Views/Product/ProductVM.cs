using Electronics.Application.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Electronics.Presentation.Areas.Admin.Views.Product
{
    public class ProductVM
    {
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
        public ProductDto Product { get; set; } = new ProductDto();
        public IEnumerable<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ProductTags { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Discounts { get; set; } = new List<SelectListItem>();
    }
}
