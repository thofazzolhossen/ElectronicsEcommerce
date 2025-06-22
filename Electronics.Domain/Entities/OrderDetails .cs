﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electronics.Domain.Entities
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name= "Order")]
        public int OrderId { get; set; }
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
