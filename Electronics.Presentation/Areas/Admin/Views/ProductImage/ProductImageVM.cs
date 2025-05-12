using Microsoft.AspNetCore.Mvc.Rendering;

namespace Electronics.Presentation.Areas.Admin.Views.ProductImage
{
    public class ProductImageVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<SelectListItem>? ProductList { get; set; }
        public string? ImagePath { get; set; }
    }
}
