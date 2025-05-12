using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Electronics.Application.Product
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Required]
        [Display(Name = "Product Tag")]
        public int ProductTagId { get; set; }

        [Display(Name = "Discount")]
        public int? DiscountId { get; set; }

        public string? ProductTypeName { get; set; }
        public string? ProductTagName { get; set; }

        public string? DiscountName { get; set; }

        public string Description { get; set; }
        public string Specification { get; set; }


        // For multiple image upload
        public List<IFormFile>? Images { get; set; }
        public List<string>? ImageUrls { get; set; } = new List<string>();


    }
}
