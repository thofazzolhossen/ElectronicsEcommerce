using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Electronics.Presentation.Areas.Admin.Views.AssignRole
{
    public class AssignRoleViewModel
    {
        [Required]
        public string? SelectedUserId { get; set; }

        [Required]
        public string? SelectedRole { get; set; }

        public List<SelectListItem>? Users { get; set; }
        public List<SelectListItem>? Roles { get; set; }
    }
}
