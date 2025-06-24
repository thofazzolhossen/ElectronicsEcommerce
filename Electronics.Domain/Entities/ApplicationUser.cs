using Microsoft.AspNetCore.Identity;
namespace Electronics.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
