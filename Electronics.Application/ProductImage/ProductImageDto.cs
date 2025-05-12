using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronics.Application.ProductImage
{
    public class ProductImageDto
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public string? ImagePath { get; set; }


    }
}
