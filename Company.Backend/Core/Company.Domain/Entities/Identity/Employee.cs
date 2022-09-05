using Microsoft.AspNetCore.Identity;

namespace Company.Domain.Entities.Identity
{
    public class Employee : IdentityUser
    {
        public const string Administrator = "Admin";

        public const string DefaultAdminPassword = "AdPAss_123";

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Position { get; set; }

        public DateTime Birthday { get; set; }

        public long Phone { get; set; }

        public string Address { get; set; }
    }
}
