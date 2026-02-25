
using Microsoft.AspNetCore.Identity;

namespace ShamsAlShamoos02.Shared.Entities
{
    public class ApplicationRoles:IdentityRole
    {
        public string RoleLevel { get; set; }
        public string Description { get; set; }
    }
}
