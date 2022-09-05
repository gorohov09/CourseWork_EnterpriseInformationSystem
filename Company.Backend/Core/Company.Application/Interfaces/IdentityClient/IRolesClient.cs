using Company.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Company.Application.Interfaces.IdentityClient
{
    public interface IRolesClient : IRoleStore<Role>
    {
    }
}
