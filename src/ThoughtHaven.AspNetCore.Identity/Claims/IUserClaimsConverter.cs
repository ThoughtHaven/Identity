using System.Security.Claims;
using System.Threading.Tasks;
using ThoughtHaven.AspNetCore.Identity.Keys;

namespace ThoughtHaven.AspNetCore.Identity.Claims
{
    public interface IUserClaimsConverter<TUser>
    {
        Task<ClaimsPrincipal> Convert(TUser user);
        Task<UserKey> Convert(ClaimsPrincipal principal);
    }
}