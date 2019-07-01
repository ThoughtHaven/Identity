using ThoughtHaven.AspNetCore.Identity.Keys;
using ThoughtHaven.Data;

namespace ThoughtHaven.AspNetCore.Identity.Internal
{
    public interface IIdentityServiceBase<TUser>
        : IAuthenticationService<TUser>, ICrudStore<UserKey, TUser>
        where TUser : class
    { }
}