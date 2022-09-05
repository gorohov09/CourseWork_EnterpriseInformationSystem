using Company.Application.DTO.Identity;
using Company.Domain.Entities.Identity;
using Company.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Company.WebAPI.Controllers.Identity
{
    [Route("api/v1/employees")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<Employee, Role, CompanyDB> _UserStore;
        public UsersApiController(CompanyDB db)
        {
            _UserStore = new UserStore<Employee, Role, CompanyDB>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Employee>> GetAll() => await _UserStore.Users.ToArrayAsync();

        #region Users

        [HttpPost("UserId")] //POST: api/v1/users/UserId
        public async Task<string> GetUserIdAsync([FromBody] Employee user) => await _UserStore.GetUserIdAsync(user);

        [HttpPost("UserName")]
        public async Task<string> GetUserNameAsync([FromBody] Employee user) => await _UserStore.GetUserNameAsync(user);

        [HttpPost("UserName/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] Employee user, string name)
        {
            await _UserStore.SetUserNameAsync(user, name);
            await _UserStore.UpdateAsync(user);
            return user.UserName;
        }

        [HttpPost("NormalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] Employee user) => await _UserStore.GetNormalizedUserNameAsync(user);

        [HttpPost("NormalUserName/{name}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] Employee user, string name)
        {
            await _UserStore.SetNormalizedUserNameAsync(user, name);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedUserName;
        }

        [HttpPost("Employee")]
        public async Task<bool> CreateAsync([FromBody] Employee user)
        {
            var creation_result = await _UserStore.CreateAsync(user);
            return creation_result.Succeeded;
        }

        [HttpPut("Employee")]
        public async Task<bool> UpdateAsync([FromBody] Employee user)
        {
            var update_result = await _UserStore.UpdateAsync(user);
            return update_result.Succeeded;
        }

        [HttpPost("Employee/Delete")]
        public async Task<bool> DeleteAsync([FromBody] Employee user)
        {
            var delete_result = await _UserStore.DeleteAsync(user);
            return delete_result.Succeeded;
        }

        [HttpGet("Employee/Find/{id}")]
        public async Task<Employee> FindByIdAsync(string id) => await _UserStore.FindByIdAsync(id);

        [HttpGet("Employee/Normal/{name}")]
        public async Task<Employee> FindByNameAsync(string name) => await _UserStore.FindByNameAsync(name);

        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] Employee user, string role)
        {
            await _UserStore.AddToRoleAsync(user, role);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpDelete("Role/{role}")]
        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] Employee user, string role)
        {
            await _UserStore.RemoveFromRoleAsync(user, role);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("Roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] Employee user) => await _UserStore.GetRolesAsync(user);

        [HttpPost("InRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] Employee user, string role) => await _UserStore.IsInRoleAsync(user, role);

        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<Employee>> GetUsersInRoleAsync(string role) => await _UserStore.GetUsersInRoleAsync(role);

        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] Employee user) => await _UserStore.GetPasswordHashAsync(user);

        [HttpPost("SetPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hash)
        {
            await _UserStore.SetPasswordHashAsync(hash.Employee, hash.Hash);
            await _UserStore.UpdateAsync(hash.Employee);
            return hash.Employee.PasswordHash;
        }

        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] Employee user) => await _UserStore.HasPasswordAsync(user);

        #endregion

        #region Claims

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] Employee user) => await _UserStore.GetClaimsAsync(user);

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO ClaimInfo)
        {
            await _UserStore.AddClaimsAsync(ClaimInfo.Employee, ClaimInfo.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO ClaimInfo)
        {
            await _UserStore.ReplaceClaimAsync(ClaimInfo.Employee, ClaimInfo.Claim, ClaimInfo.NewClaim);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveClaim")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO ClaimInfo)
        {
            await _UserStore.RemoveClaimsAsync(ClaimInfo.Employee, ClaimInfo.Claims);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<Employee>> GetUsersForClaimAsync([FromBody] Claim claim) =>
            await _UserStore.GetUsersForClaimAsync(claim);

        #endregion

        #region TwoFactor

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] Employee user) => await _UserStore.GetTwoFactorEnabledAsync(user);

        [HttpPost("SetTwoFactor/{enable}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] Employee user, bool enable)
        {
            await _UserStore.SetTwoFactorEnabledAsync(user, enable);
            await _UserStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        #endregion

        #region Email/Phone

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] Employee user) => await _UserStore.GetEmailAsync(user);

        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] Employee user, string email)
        {
            await _UserStore.SetEmailAsync(user, email);
            await _UserStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] Employee user) => await _UserStore.GetEmailConfirmedAsync(user);

        [HttpPost("SetEmailConfirmed/{enable}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] Employee user, bool enable)
        {
            await _UserStore.SetEmailConfirmedAsync(user, enable);
            await _UserStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet("UserFindByEmail/{email}")]
        public async Task<Employee> FindByEmailAsync(string email) => await _UserStore.FindByEmailAsync(email);

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] Employee user) => await _UserStore.GetNormalizedEmailAsync(user);

        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] Employee user, string? email)
        {
            await _UserStore.SetNormalizedEmailAsync(user, email);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] Employee user) => await _UserStore.GetPhoneNumberAsync(user);

        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] Employee user, string phone)
        {
            await _UserStore.SetPhoneNumberAsync(user, phone);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] Employee user) =>
            await _UserStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] Employee user, bool confirmed)
        {
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion

        #region Login/Lockout

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO login)
        {
            await _UserStore.AddLoginAsync(login.Employee, login.UserLoginInfo);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]
        public async Task RemoveLoginAsync([FromBody] Employee user, string LoginProvider, string ProviderKey)
        {
            await _UserStore.RemoveLoginAsync(user, LoginProvider, ProviderKey);
            await _UserStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] Employee user) => await _UserStore.GetLoginsAsync(user);

        [HttpGet("Employee/FindByLogin/{LoginProvider}/{ProviderKey}")]
        public async Task<Employee> FindByLoginAsync(string LoginProvider, string ProviderKey) => await _UserStore.FindByLoginAsync(LoginProvider, ProviderKey);

        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] Employee user) => await _UserStore.GetLockoutEndDateAsync(user);

        [HttpPost("SetLockoutEndDate")]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDTO LockoutInfo)
        {
            await _UserStore.SetLockoutEndDateAsync(LockoutInfo.Employee, LockoutInfo.LockoutEnd);
            await _UserStore.UpdateAsync(LockoutInfo.Employee);
            return LockoutInfo.Employee.LockoutEnd;
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] Employee user)
        {
            var count = await _UserStore.IncrementAccessFailedCountAsync(user);
            await _UserStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task<int> ResetAccessFailedCountAsync([FromBody] Employee user)
        {
            await _UserStore.ResetAccessFailedCountAsync(user);
            await _UserStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] Employee user) => await _UserStore.GetAccessFailedCountAsync(user);

        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] Employee user) => await _UserStore.GetLockoutEnabledAsync(user);

        [HttpPost("SetLockoutEnabled/{enable}")]
        public async Task<bool> SetLockoutEnabledAsync([FromBody] Employee user, bool enable)
        {
            await _UserStore.SetLockoutEnabledAsync(user, enable);
            await _UserStore.UpdateAsync(user);
            return user.LockoutEnabled;
        }

        #endregion
    }
}
