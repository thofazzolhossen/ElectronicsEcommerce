using System.ComponentModel.DataAnnotations;

namespace Electronics.Domain.Entities
{
    public class Discount
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Discount Name")]
        public string Name { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Discount Percentage")]
        public double Percentage { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

    }
}
