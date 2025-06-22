namespace Electronics.Presentation.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public string SelectedRole { get; set; }
    }
}
