using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Price")]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public string Description { get; set; }
        public string Specification { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductType? ProductType { get; set; }

        [Required]
        [Display(Name = "Product Tag")]
        public int ProductTagId { get; set; }

        [ForeignKey("ProductTagId")]
        public ProductTag? ProductTag { get; set; }

        [Display(Name = "Discount")]
        public int? DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public Discount? Discount { get; set; }

        [NotMapped]
        public string? ImagePath { get; set; }


    }
}
