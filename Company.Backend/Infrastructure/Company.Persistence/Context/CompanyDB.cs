using Company.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Company.Persistence.Context
{
    public class CompanyDB : IdentityDbContext<Employee, Role, string>
    {
        public CompanyDB(DbContextOptions<CompanyDB> options)
            : base(options)
        {

        }
    }
}
