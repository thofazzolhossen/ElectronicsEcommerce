using System.ComponentModel.DataAnnotations;

namespace Electronics.Presentation.Areas.Admin.Views.Role
{
    public class CreateRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
