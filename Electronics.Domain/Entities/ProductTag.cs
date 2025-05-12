using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Domain.Entities
{
    public class ProductTag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Tag")]
        public string Name { get; set; }
    }
}
