using Company.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Company.Application.Interfaces.IdentityClient
{
    public interface IEmployeesClient :
        IUserRoleStore<Employee>,
        IUserPasswordStore<Employee>,
        IUserEmailStore<Employee>,
        IUserPhoneNumberStore<Employee>,
        IUserTwoFactorStore<Employee>,
        IUserLoginStore<Employee>,
        IUserClaimStore<Employee>
    {
    }
}
