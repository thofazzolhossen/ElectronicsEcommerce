using System.ComponentModel.DataAnnotations;

namespace Electronics.Presentation.Areas.Admin.Views.Role
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
