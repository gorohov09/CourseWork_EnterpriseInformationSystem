using Company.Application.DTO.Identity;
using Company.Application.Interfaces.IdentityClient;
using Company.Domain.Entities.Identity;
using Company.WebAPI.Clients.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Company.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IEmployeesClient
    {
        public UsersClient(HttpClient Client) : base(Client, "api/v1/employees")
        {
        }

        #region Implementation of IUserStore<Employee>

        public async Task<string> GetUserIdAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserId", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserName", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(Employee user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/UserName/{name}", user, cancel);
            user.UserName = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/NormalUserName/", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(Employee user, string name, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/NormalUserName/{name}", user, cancel);
            user.NormalizedUserName = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Employee", user, cancel);
            var creation_success = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return creation_success
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Employee user, CancellationToken cancel)
        {
            var response = await PutAsync($"{Address}/Employee", user, cancel);
            var update_result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return update_result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Employee/Delete", user, cancel);
            var delete_result = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);

            return delete_result
                ? IdentityResult.Success
                : IdentityResult.Failed();
        }

        public async Task<Employee> FindByIdAsync(string id, CancellationToken cancel)
        {
            return await GetAsync<Employee>($"{Address}/Employee/Find/{id}", cancel).ConfigureAwait(false);
        }

        public async Task<Employee> FindByNameAsync(string name, CancellationToken cancel)
        {
            return await GetAsync<Employee>($"{Address}/Employee/Normal/{name}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserRoleStore<Employee>

        public async Task AddToRoleAsync(Employee user, string role, CancellationToken cancel)
        {
            await PostAsync($"{Address}/Role/{role}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(Employee user, string role, CancellationToken cancel)
        {
            await PostAsync($"{Address}/Role/Delete/{role}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/roles", user, cancel).ConfigureAwait(false);
            return (await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<IList<string>>(cancellationToken: cancel)
               .ConfigureAwait(false))!;
        }

        public async Task<bool> IsInRoleAsync(Employee user, string role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/InRole/{role}", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<IList<Employee>> GetUsersInRoleAsync(string role, CancellationToken cancel)
        {
            return (await GetAsync<List<Employee>>($"{Address}/UsersInRole/{role}", cancel).ConfigureAwait(false))!;
        }

        #endregion

        #region Implementation of IUserPasswordStore<Employee>

        public async Task SetPasswordHashAsync(Employee user, string hash, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPasswordHash", new PasswordHashDTO { Employee = user, Hash = hash }, cancel)
               .ConfigureAwait(false);
            user.PasswordHash = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel);
        }

        public async Task<string> GetPasswordHashAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPasswordHash", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> HasPasswordAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/HasPassword", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserEmailStore<Employee>

        public async Task SetEmailAsync(Employee user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmail/{email}", user, cancel).ConfigureAwait(false);
            user.Email = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetEmailAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmail", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmailConfirmed", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(Employee user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.EmailConfirmed = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<Employee> FindByEmailAsync(string email, CancellationToken cancel)
        {
            return (await GetAsync<Employee>($"{Address}/Employee/FindByEmail/{email}", cancel).ConfigureAwait(false))!;
        }

        public async Task<string> GetNormalizedEmailAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Employee/GetNormalizedEmail", user, cancel);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(Employee user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedEmail/{email}", user, cancel).ConfigureAwait(false);
            user.NormalizedEmail = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserPhoneNumberStore<Employee>

        public async Task SetPhoneNumberAsync(Employee user, string phone, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumber/{phone}", user, cancel).ConfigureAwait(false);
            user.PhoneNumber = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumber", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadAsStringAsync(cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(Employee user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, cancel).ConfigureAwait(false);
            user.PhoneNumberConfirmed = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserLoginStore<Employee>

        public async Task AddLoginAsync(Employee user, UserLoginInfo login, CancellationToken cancel)
        {
            await PostAsync($"{Address}/AddLogin", new AddLoginDTO { Employee = user, UserLoginInfo = login }, cancel).ConfigureAwait(false);
        }

        public async Task RemoveLoginAsync(Employee user, string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            await PostAsync($"{Address}/RemoveLogin/{LoginProvider}/{ProviderKey}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetLogins", user, cancel).ConfigureAwait(false);
            return (await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<UserLoginInfo>>(cancellationToken: cancel)
               .ConfigureAwait(false))!;
        }

        public async Task<Employee> FindByLoginAsync(string LoginProvider, string ProviderKey, CancellationToken cancel)
        {
            return (await GetAsync<Employee>($"{Address}/Employee/FindByLogin/{LoginProvider}/{ProviderKey}", cancel).ConfigureAwait(false))!;
        }

        #endregion

        #region Implementation of IUserLockoutStore<Employee>

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetLockoutEndDate", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEndDateAsync(Employee user, DateTimeOffset? EndDate, CancellationToken cancel)
        {
            var response = await PostAsync(
                    $"{Address}/SetLockoutEndDate",
                    new SetLockoutDTO { Employee = user, LockoutEnd = EndDate },
                    cancel)
               .ConfigureAwait(false);
            user.LockoutEnd = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<DateTimeOffset?>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/IncrementAccessFailedCount", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<int>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task ResetAccessFailedCountAsync(Employee user, CancellationToken cancel)
        {
            await PostAsync($"{Address}/ResetAccessFailedCont", user, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetAccessFailedCountAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetAccessFailedCount", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<int>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetLockoutEnabledAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetLockoutEnabled", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task SetLockoutEnabledAsync(Employee user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetLockoutEnabled/{enabled}", user, cancel).ConfigureAwait(false);
            user.LockoutEnabled = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserTwoFactorStore<Employee>

        public async Task SetTwoFactorEnabledAsync(Employee user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetTwoFactor/{enabled}", user, cancel).ConfigureAwait(false);
            user.TwoFactorEnabled = await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetTwoFactorEnabled", user, cancel).ConfigureAwait(false);
            return await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<bool>(cancellationToken: cancel)
               .ConfigureAwait(false);
        }

        #endregion

        #region Implementation of IUserClaimStore<Employee>

        public async Task<IList<Claim>> GetClaimsAsync(Employee user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetClaims", user, cancel).ConfigureAwait(false);
            return (await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<Claim>>(cancellationToken: cancel)
               .ConfigureAwait(false))!;
        }

        public async Task AddClaimsAsync(Employee user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                    $"{Address}/AddClaims",
                    new AddClaimDTO { Employee = user, Claims = claims },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task ReplaceClaimAsync(Employee user, Claim OldClaim, Claim NewClaim, CancellationToken cancel)
        {
            await PostAsync(
                    $"{Address}/ReplaceClaim",
                    new ReplaceClaimDTO { Employee = user, Claim = OldClaim, NewClaim = NewClaim },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task RemoveClaimsAsync(Employee user, IEnumerable<Claim> claims, CancellationToken cancel)
        {
            await PostAsync(
                    $"{Address}/RemoveClaims",
                    new RemoveClaimDTO { Employee = user, Claims = claims },
                    cancel)
               .ConfigureAwait(false);
        }

        public async Task<IList<Employee>> GetUsersForClaimAsync(Claim claim, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetUsersForClaim", claim, cancel).ConfigureAwait(false);
            return (await response
               .EnsureSuccessStatusCode()
               .Content
               .ReadFromJsonAsync<List<Employee>>(cancellationToken: cancel)
               .ConfigureAwait(false))!;
        }

        #endregion
    }
}
