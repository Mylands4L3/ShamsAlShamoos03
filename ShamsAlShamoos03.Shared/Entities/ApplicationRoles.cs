
using Microsoft.AspNetCore.Identity;

namespace ShamsAlShamoos03.Shared.Entities
{
    public class ApplicationRoles:IdentityRole
    {
        public string RoleLevel { get; set; }
        public string Description { get; set; }
    }
}
