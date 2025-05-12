using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Domain.Entities
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public string Name { get; set; }
    }
}
