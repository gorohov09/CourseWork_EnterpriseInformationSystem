using Microsoft.AspNetCore.Identity;

namespace Company.Domain.Entities.Identity
{
    public class Role : IdentityRole
    {
        public const string Administrators = "Administrators";

        public const string Employees = "Employees";

        public string Description { get; set; }
    }
}
