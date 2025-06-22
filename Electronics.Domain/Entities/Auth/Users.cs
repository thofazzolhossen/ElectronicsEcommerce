using Microsoft.AspNet.Identity.EntityFramework;

namespace Electronics.Domain.Entities.Auth
{
    public class Users: IdentityUser
    {
        public string FullName { get; set; }
    }
}
